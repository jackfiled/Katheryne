using System.Text.Json;
using Katheryne.Abstractions;

namespace Katheryne.Modules;

/// <summary>
/// 参数模块实现基类
/// </summary>
public abstract class ModuleBase : IParamsModule
{
    protected readonly Dictionary<string, Func<string>> Functions = new();

    protected readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public abstract string ModuleName { get; }

    public string this[string param]
    {
        get
        {
            Func<string> func = Functions[param];
            return func();
        }
    }

    public bool ContainsParam(string param)
    {
        return Functions.ContainsKey(param);
    }
}