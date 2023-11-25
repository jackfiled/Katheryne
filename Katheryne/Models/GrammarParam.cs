namespace Katheryne.Models;

/// <summary>
/// 语法中的模块参数
/// </summary>
/// <param name="OriginString">原始字符串</param>
/// <param name="Module">模块名称</param>
/// <param name="Param">参数</param>
public record GrammarParam(string OriginString, string Module, string Param);