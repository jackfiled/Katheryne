using Katheryne.Abstractions;
using Katheryne.Models;
using Microsoft.Extensions.Logging;
using YamlDotNet.Serialization;

namespace Katheryne.Services;

public class KatheryneChatRobotFactory
{
    private readonly YamlDeserializerFactory _deserializerFactory;
    private readonly ILogger<KatheryneChatRobotFactory> _factoryLogger;
    private readonly ILogger<KatheryneChatRobot> _robotLogger;
    private readonly DefaultChatRobot _defaultChatRobot;

    private Grammar? _grammar;

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

    public void SetGrammar(string grammarText)
    {
        _factoryLogger.LogInformation("Receive new grammar: {}.", grammarText);
        GrammarText = grammarText;
        IDeserializer deserializer = _deserializerFactory.GetDeserializer();

        LexicalModel model = deserializer.Deserialize<LexicalModel>(grammarText);
        _grammar = new Grammar(new GrammarTree(model), model.RobotName, model.BeginStageName);
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