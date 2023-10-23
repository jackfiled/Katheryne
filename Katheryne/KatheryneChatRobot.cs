using System.Text.RegularExpressions;
using Katheryne.Abstractions;
using Katheryne.Models;
using Microsoft.Extensions.Logging;

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
            _grammarTree[_currentStage].Answer
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
        List<string> result = new();

        foreach (InnerTransformer transformer in _grammarTree[_currentStage].Transformers)
        {
            if (transformer.Pattern is null)
            {
                continue;
            }
            
            Match match = transformer.Pattern.Match(input);

            if (match.Success)
            {
                _currentStage = transformer.NextStage;
                result.Add(_grammarTree[_currentStage].Answer);
                _logger.LogDebug("Moving to stage {}.", _currentStage);
            }
        }
        
        EmptyTransform(result);
        return result;
    }

    /// <summary>
    /// 进行当前阶段的空转移
    /// </summary>
    /// <param name="result">存放输出回答的列表</param>
    private void EmptyTransform(List<string> result)
    {
        var flag = true;
        while (flag)
        {
            flag = false;
            foreach (InnerTransformer transformer in _grammarTree[_currentStage].Transformers)
            {
                if (string.IsNullOrEmpty(transformer.RowPattern))
                {
                    flag = true;
                    _currentStage = transformer.NextStage;
                    result.Add(_grammarTree[_currentStage].Answer);
                    
                    _logger.LogDebug("Moving to stage {} with empty transform.", _currentStage);
                    break;
                }
            }
        }
    }
}