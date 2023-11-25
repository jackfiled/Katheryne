namespace Katheryne.Abstractions;

/// <summary>
/// 参数实现模块
/// </summary>
public interface IParamsModule
{
    /// <summary>
    /// 模块的名称
    /// </summary>
    public string ModuleName { get; }

    /// <summary>
    /// 获得模块中特定指定参数的文本
    /// </summary>
    /// <param name="param">指定参数</param>
    public string this[string param] { get; }

    /// <summary>
    /// 判断模块是否提供指定参数
    /// </summary>
    /// <param name="param">指定参数</param>
    /// <returns>如果为真，说明模块提供该参数，反之没有提供</returns>
    public bool ContainsParam(string param);
}