using System.Text.Json;
using Microsoft.Extensions.Caching.Memory;

namespace WeatherApi.Services
{
    public class WeatherService
    {
        private readonly HttpClient _httpClient;
        private readonly IMemoryCache _cache;
        private readonly string _apiKey;

        public WeatherService(IMemoryCache cache, IConfiguration configuration)
        {
            _httpClient = new HttpClient();
            _cache = cache;
            _apiKey = configuration["WeatherApi:ApiKey"] ?? throw new ArgumentNullException("API key is missing"); // 從 appsettings 或環境變數讀取
        }

        public async Task<object> GetWeatherAsync(string city)
        {
            // 嘗試從快取取得資料
            if (_cache.TryGetValue(city, out object cachedWeather))
            {
                return cachedWeather;
            }

            // 沒有快取，就去 OpenWeatherMap 拉取
            string url = $"https://api.openweathermap.org/data/2.5/weather?q={city}&appid={_apiKey}&units=metric&lang=zh_tw";
            
            try
            {
                var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();

                using JsonDocument doc = JsonDocument.Parse(json);
                var root = doc.RootElement;

                var result = new
                {
                    City = root.GetProperty("name").GetString(),
                    Temperature = root.GetProperty("main").GetProperty("temp").GetDouble(),
                    Condition = root.GetProperty("weather")[0].GetProperty("main").GetString()
                };

                // 設定快取（有效時間設定為 10 分鐘）
                _cache.Set(city, result, TimeSpan.FromMinutes(10));

                return result;
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"取得天氣資料失敗: {ex.Message}");
            }
        }
    }
}