using Katheryne.Modules;
using Xunit.Abstractions;

namespace Katheryne.Tests.Modules;

public class WeatherModuleTests
{
    private readonly ITestOutputHelper _output;

    public WeatherModuleTests(ITestOutputHelper output)
    {
        _output = output;
    }

    [Fact]
    public void WeatherModuleTest()
    {
        var weather = new WeatherModule();

        Assert.True(weather.ContainsParam("text"));
        Assert.True(weather.ContainsParam("temp"));

        _output.WriteLine(weather["text"]);
        _output.WriteLine(weather["temp"]);
    }
}