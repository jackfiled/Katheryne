using Katheryne.Abstractions;
using Katheryne.Models;
using Katheryne.Services;
using Microsoft.Extensions.Logging;
using YamlDotNet.Serialization;

namespace Katheryne;

public class KatheryneChatRobot : IChatRobot
{
    private readonly ILogger<KatheryneChatRobot> _logger;
    private readonly GrammarTree _grammarTree;

    private string _currentStage;

    public KatheryneChatRobot(GrammarTree grammarTree, ILogger<KatheryneChatRobot> logger, 
        string beginStage, string robotName)
    {
        _logger = logger;

        _grammarTree = grammarTree;
        _currentStage = beginStage;
        RobotName = robotName;
    }
    
    public string RobotName { get; }
    public IEnumerable<string> OnChatStart()
    {
        return new[]
        {
            _grammarTree[_currentStage]
        };
    }

    public IEnumerable<string> OnChatStop()
    {
        return new[]
        {
            "再见。"
        };
    }

    public IEnumerable<string> ChatNext(string input)
    {
        _logger.LogDebug("Receive input {} on stage {}.", input, _currentStage);
        (_currentStage, string answer) = _grammarTree.NextStage(_currentStage, input);
        _logger.LogDebug("Change stage to {}.", _currentStage);

        return new[]
        {
            answer
        };
    }
}