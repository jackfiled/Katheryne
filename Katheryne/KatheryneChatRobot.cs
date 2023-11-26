using System.Text.RegularExpressions;
using Katheryne.Abstractions;
using Katheryne.Exceptions;
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
            _grammarTree[_currentStage].Answer.RowString
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
                StringFormatter answer = _grammarTree[_currentStage].Answer;
                if (answer.IsFormat)
                {
                    string temp = answer.Format(match.Groups);
                    _logger.LogDebug("Format answer {} to {}.",
                        answer.RowString, temp);
                    result.Add(temp);
                }
                else
                {
                    result.Add(answer.RowString);
                }
                _logger.LogDebug("Moving to stage {} on input {}.",
                    _currentStage, input);
                break;
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
        // 经过的状态集合, 避免递归循环
        HashSet<string> movedStages = new();
        movedStages.Add(_currentStage);

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
                    if (movedStages.Contains(_currentStage))
                    {
                        // 发生递归调用
                        throw new GrammarException("Recursively transform detected!");
                    }
                    else
                    {
                        movedStages.Add(_currentStage);
                    }

                    result.Add(_grammarTree[_currentStage].Answer.RowString);
                    
                    _logger.LogDebug("Moving to stage {} with empty transform.",
                        _currentStage);
                    break;
                }
            }
        }
    }
}