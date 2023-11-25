using Katheryne.Abstractions;
using Katheryne.Exceptions;
using Katheryne.Models;
using Microsoft.Extensions.Logging;
using YamlDotNet.Serialization;

namespace Katheryne.Services;

/// <summary>
/// Katheryne 聊天机器人工厂实现
/// </summary>
public class KatheryneChatRobotFactory : IChatRobotFactory
{
    private readonly YamlDeserializerFactory _deserializerFactory;
    private readonly ILogger<KatheryneChatRobotFactory> _factoryLogger;
    private readonly ILogger<KatheryneChatRobot> _robotLogger;
    private readonly DefaultChatRobot _defaultChatRobot;

    private Grammar? _grammar;

    public Dictionary<string, IParamsModule> Modules { get; } = new();

    public string GrammarText { get; private set; } = string.Empty;

    public KatheryneChatRobotFactory(YamlDeserializerFactory deserializerFactory,
        ILogger<KatheryneChatRobotFactory> factoryLogger,
        ILogger<KatheryneChatRobot> robotLogger,
        DefaultChatRobot defaultChatRobot)
    {
        _deserializerFactory = deserializerFactory;
        _factoryLogger = factoryLogger;
        _robotLogger = robotLogger;
        _defaultChatRobot = defaultChatRobot;
    }

    /// <summary>
    /// 设置当前机器人使用的文法
    /// </summary>
    /// <param name="grammarText">文法字符串</param>
    /// <exception cref="GrammarException">编译文法失败抛出的异常</exception>
    public void SetGrammar(string grammarText)
    {
        _factoryLogger.LogDebug("Receive new grammar: {}.", grammarText);
        GrammarText = grammarText;
        IDeserializer deserializer = _deserializerFactory.GetDeserializer();

        LexicalModel model;
        try
        {
            model = deserializer.Deserialize<LexicalModel>(grammarText);
        }
        catch (Exception ex)
        {
            throw new GrammarException("Failed to parse lexical model.", ex);
        }
        _grammar = new Grammar(new GrammarTree(model, Modules),
            model.RobotName, model.BeginStageName);
    }

    public IChatRobot GetRobot()
    {
        if (_grammar is null)
        {
            _factoryLogger.LogDebug("Get default chat robot.");
            return _defaultChatRobot;
        }

        _factoryLogger.LogDebug("Get chat robot: {}.", _grammar.RobotName);
        return new KatheryneChatRobot(_grammar.GrammarTree, _robotLogger,
            _grammar.BeginStage, _grammar.RobotName);
    }

    private record Grammar(GrammarTree GrammarTree, string RobotName, string BeginStage);
}