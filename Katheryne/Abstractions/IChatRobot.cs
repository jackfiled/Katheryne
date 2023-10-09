namespace Katheryne.Abstractions;

/// <summary>
/// 对话机器人接口
/// </summary>
public interface IChatRobot
{
    public string RobotName { get; }
    
    public IEnumerable<string> OnChatStart();

    public IEnumerable<string> OnChatStop();
    
    
}