using Katheryne.Abstractions;

namespace Katheryne.Tests.Mocks;

public class MockWeatherModule : IParamsModule
{
    private readonly Dictionary<string, string> _param = new()
    {
        { "temp", "20" },
        { "text", "晴" }
    };

    public string ModuleName => "weather";

    public string this[string param] => _param[param];

    public bool ContainsParam(string param)
    {
        return _param.ContainsKey(param);
    }
}