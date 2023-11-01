using System.Net.Http.Json;
using System.Text.Json.Serialization;
using Katheryne.Exceptions;

namespace Katheryne.Modules;

public class WeatherModule : ModuleBase
{
    private static readonly HttpClient s_httpClient = new();

    private const string WeatherApi =
        "https://api.seniverse.com/v3/weather/now.json?key=S7s93MkxJ1q7mgHoj&location=beijing&language=zh-Hans&unit=c";

    private WeatherDto? _weatherDto;

    public override string ModuleName => "weather";

    public WeatherModule()
    {
        Functions.Add("text", () => FetchWeatherDate().Now.Text);
        Functions.Add("temp", () => FetchWeatherDate().Now.Temperature);
    }

    private WeatherDto FetchWeatherDate()
    {
        if (_weatherDto is not null)
        {
            if (DateTime.Now - _weatherDto.LastUpdate < TimeSpan.FromMinutes(5))
            {
                return _weatherDto;
            }
        }

        try
        {
            Task<Dictionary<string, List<WeatherDto>>?> task = s_httpClient.GetFromJsonAsync<Dictionary<string, List<WeatherDto>>>(
                WeatherApi, JsonOptions);
            task.Wait();
            Dictionary<string, List<WeatherDto>>? response = task.Result;
            WeatherDto? weather = response?["results"][0];

            _weatherDto = weather ?? throw new ModuleException("Failed to fetch weather data.");
            return _weatherDto;
        }
        catch (HttpRequestException e)
        {
            throw new ModuleException("Failed to fetch weather data.", e);
        }
    }

    private class WeatherNowDto
    {
        public required string Text { get; set; }

        public required string Code { get; set; }

        public required string Temperature { get; set; }
    }

    private class WeatherDto
    {
        public required WeatherNowDto Now { get; set; }

        [JsonPropertyName("last_update")]
        public DateTime LastUpdate { get; set; }
    }
}