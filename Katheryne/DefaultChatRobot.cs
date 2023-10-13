using Katheryne.Abstractions;
using Microsoft.Extensions.Logging;

namespace Katheryne;

public class DefaultChatRobot : IChatRobot
{
    private readonly ILogger<DefaultChatRobot> _logger;

    public DefaultChatRobot(ILogger<DefaultChatRobot> logger)
    {
        _logger = logger;
    }
    
    public string RobotName => "Default";
    
    public IEnumerable<string> OnChatStart()
    {
        _logger.LogDebug("Start default chat robot.");
        return new[]
        {
            "坏了，被你发现默认机器人了。",
            "使用这个粪机器人，怎么能得高分呢？",
            "必须要出重拳！"
        };
    }

    public IEnumerable<string> OnChatStop()
    {
        _logger.LogDebug("End default chat robot.");
        return new[]
        {
            "我不到啊。"
        };
    }

    public IEnumerable<string> ChatNext(string input)
    {
        _logger.LogDebug("Robot receive message: \"{}\".", input);
        return new[]
        {
            "啊对对对。"
        };
    }
}