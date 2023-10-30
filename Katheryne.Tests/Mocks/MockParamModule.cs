using Katheryne.Abstractions;

namespace Katheryne.Tests.Mocks;

public class MockParamModule : IParamsModule
{
    private readonly Dictionary<string, Func<string>> _functions = new();

    public string ModuleName => "test";

    public MockParamModule()
    {
        _functions.Add("hello", Hello);
    }

    public string this[string param]
    {
        get
        {
            Func<string> func = _functions[param];
            return func();
        }
    }

    public bool ContainsParam(string param)
    {
        return _functions.ContainsKey(param);
    }

    private string Hello()
    {
        return "Hello, Katheryne";
    }
}