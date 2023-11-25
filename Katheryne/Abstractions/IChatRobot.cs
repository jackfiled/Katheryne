namespace Katheryne.Abstractions;

/// <summary>
/// 对话机器人接口
/// </summary>
public interface IChatRobot
{
    /// <summary>
    /// 定义机器人的名称
    /// </summary>
    public string RobotName { get; }

    /// <summary>
    /// 机器人在启动时输出的对话
    /// </summary>
    /// <returns>对话列表</returns>
    public IEnumerable<string> OnChatStart();

    /// <summary>
    /// 机器人在结束对话时输出的对话列表
    /// </summary>
    /// <returns>对话列表</returns>
    public IEnumerable<string> OnChatStop();

    /// <summary>
    /// 机器人在获得用户输入时输出的对话列表
    /// </summary>
    /// <param name="input">用户的输入</param>
    /// <returns>机器人输出的对话列表</returns>
    public IEnumerable<string> ChatNext(string input);
}