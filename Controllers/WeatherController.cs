using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using WeatherApi.Services;

namespace WeatherApi.Controllers
{
    //假資料範例
    // [ApiController]
    // [Route("api/[controller]")]
    // public class WeatherController : ControllerBase
    // {
    //     [HttpGet("{city}")]
    //     public IActionResult GetWeather(string city)
    //     {
    //         // 假資料，模擬不同城市的天氣
    //         var weatherData = new Dictionary<string, object>
    //         {
    //             { "London", new { Temperature = 15, Condition = "Cloudy" } },
    //             { "Taipei", new { Temperature = 28, Condition = "Sunny" } },
    //             { "Tokyo", new { Temperature = 22, Condition = "Rainy" } }
    //         };

    //         if (weatherData.ContainsKey(city))
    //         {
    //             return Ok(weatherData[city]);
    //         }
    //         else
    //         {
    //             return NotFound(new { Message = $"找不到 {city} 的天氣資料" });
    //         }
    //     }
    // }

    //實際串API
    // [ApiController]
    // [Route("api/[controller]")]
    // public class WeatherController : ControllerBase
    // {
    //     private readonly HttpClient _httpClient;
    //     private readonly string _apiKey = "914e47d66cefdeee0a9804890c07b83d"; // 👈 記得改成你申請的API KEY！

    //     public WeatherController()
    //     {
    //         _httpClient = new HttpClient();
    //     }

    //     [HttpGet("{city}")]
    //     public async Task<IActionResult> GetWeather(string city)
    //     {
    //         string url = $"https://api.openweathermap.org/data/2.5/weather?q={city}&appid={_apiKey}&units=metric&lang=zh_tw";

    //         try
    //         {
    //             var response = await _httpClient.GetAsync(url);
    //             response.EnsureSuccessStatusCode();

    //             var json = await response.Content.ReadAsStringAsync();

    //             // 這裡我們只挑幾個欄位來解析（可以再擴充）
    //             using JsonDocument doc = JsonDocument.Parse(json);
    //             var root = doc.RootElement;

    //             var result = new
    //             {
    //                 City = root.GetProperty("name").GetString(),
    //                 Temperature = root.GetProperty("main").GetProperty("temp").GetDouble(),
    //                 Condition = root.GetProperty("weather")[0].GetProperty("main").GetString()
    //             };

    //             return Ok(result);
    //         }
    //         catch (HttpRequestException ex)
    //         {
    //             return StatusCode(500, new { Message = "取得天氣資料失敗", Error = ex.Message });
    //         }
    //     }
    // }

    //包成 Service 分層
    [ApiController]
    [Route("api/[controller]")]
    public class WeatherController : ControllerBase
    {
        private readonly WeatherService _weatherService;

        public WeatherController(WeatherService weatherService)
        {
            _weatherService = weatherService;
        }

        [HttpGet("{city}")]
        public async Task<IActionResult> GetWeather(string city)
        {
            try
            {
                var weather = await _weatherService.GetWeatherAsync(city);
                return Ok(weather);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });
            }
        }
    }
}