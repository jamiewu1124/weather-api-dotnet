using WeatherApi.Services;

var builder = WebApplication.CreateBuilder(args);
// 加入 Swagger 支援
builder.Services.AddEndpointsApiExplorer();
//客製化 Swagger 文件標題
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() {
        Title = "Weather API",
        Version = "v1",
        Description = "這是一個使用 OpenWeatherMap API 的天氣查詢服務"
    });
});

// 其他註冊...
builder.Services.AddControllers();
builder.Services.AddMemoryCache();
builder.Services.AddScoped<WeatherService>();

var app = builder.Build();
// 如果是開發環境，就啟用 Swagger 頁面
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();
app.Run();

