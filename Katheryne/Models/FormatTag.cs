namespace Katheryne.Models;

/// <summary>
/// 格式化标记
/// </summary>
/// <param name="Value">原始字符串</param>
/// <param name="Index">在匹配结果中的序号</param>
public record FormatTag(string Value, int Index);