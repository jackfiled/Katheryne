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
    
    public string RobotName => "凯瑟琳";
    
    public IEnumerable<string> OnChatStart()
    {
        _logger.LogDebug("Start default chat robot.");
        return new[]
        {
            "向着星辰与深渊！",
            "欢迎来到冒险家协会。"
        };
    }

    public IEnumerable<string> OnChatStop()
    {
        _logger.LogDebug("End default chat robot.");
        return new[]
        {
            "再见，感谢您对协会做出的贡献，冒险家。"
        };
    }

    public IEnumerable<string> ChatNext(string input)
    {
        _logger.LogDebug("Robot receive message: \"{}\".", input);
        return new[]
        {
            "暂时不支持该功能，请联系维护人员。"
        };
    }
}