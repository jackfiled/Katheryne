namespace Katheryne.Abstractions;

/// <summary>
/// 参数实现模块
/// </summary>
public interface IParamsModule
{
    public string ModuleName { get; }

    public string this[string param] { get; }

    public bool ContainsParam(string param);
}