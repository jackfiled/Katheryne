using Katheryne.Exceptions;

namespace Katheryne.Abstractions;

/// <summary>
/// 聊天机器人接口
/// </summary>
public interface IChatRobotFactory
{
    /// <summary>
    /// 当前使用的语法文本
    /// </summary>
    public string GrammarText { get; }

    /// <summary>
    /// 设置当前机器人使用的文法
    /// </summary>
    /// <param name="grammarText">文法字符串</param>
    /// <exception cref="GrammarException">编译文法失败抛出的异常</exception>
    public void SetGrammar(string grammarText);

    /// <summary>
    /// 获得聊天机器人
    /// </summary>
    /// <returns>使用当前文法的机器人</returns>
    public IChatRobot GetRobot();
}