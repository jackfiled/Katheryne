using Katheryne.Abstractions;
using Katheryne.Exceptions;
using Katheryne.Modules;
using Katheryne.Services;
using Katheryne.Tests.Mocks;
using Microsoft.Extensions.Logging;
using Moq;

namespace Katheryne.Tests.Katheryne;

public class KatheryneRobotTests
{
    private readonly Mock<ILogger<DefaultChatRobot>> _defaultChatRobotLogger = new();
    private readonly Mock<ILogger<KatheryneChatRobot>> _katheryneChatRobotLogger = new();
    private readonly Mock<ILogger<KatheryneChatRobotFactory>> _katheryneChatRobotFactoryLogger = new();
    private readonly DefaultChatRobot _defaultChatRobot;
    private readonly KatheryneChatRobotFactory _katheryneChatRobotFactory;

    public KatheryneRobotTests()
    {
        _defaultChatRobot = new DefaultChatRobot(_defaultChatRobotLogger.Object);
        _katheryneChatRobotFactory = new KatheryneChatRobotFactory(new YamlDeserializerFactory(),
            _katheryneChatRobotFactoryLogger.Object,
            _katheryneChatRobotLogger.Object,
            _defaultChatRobot);
    }

    [Fact]
    public void DefaultRobotTest()
    {
        InputOutputFile file = new("DefaultRobot");
        ValidateOutput(_defaultChatRobot, file);
    }

    [Fact]
    public void FactoryDefaultRobotTest()
    {
        InputOutputFile file = new("DefaultRobot");
        ValidateOutput(_katheryneChatRobotFactory.GetRobot(), file);
    }

    [Fact]
    public void KatheryneRobotTest1()
    {
        InputOutputFile file = new("Grammar1");
        StreamReader reader = new(Path.Combine(file.PrefixPath, "grammar.yaml"));
        _katheryneChatRobotFactory.SetGrammar(reader.ReadToEnd());

        ValidateOutput(_katheryneChatRobotFactory.GetRobot(), file);
    }

    [Fact]
    public void KatheryneRobotTest2()
    {
        InputOutputFile file = new("Grammar2");
        StreamReader reader = new(Path.Combine(file.PrefixPath, "grammar.yaml"));
        _katheryneChatRobotFactory.SetGrammar(reader.ReadToEnd());

        ValidateOutput(_katheryneChatRobotFactory.GetRobot(), file);
    }

    [Fact]
    public void RecursivelyExceptionTest()
    {
        IParamsModule weatherModule = new MockWeatherModule();
        _katheryneChatRobotFactory.Modules.Clear();
        _katheryneChatRobotFactory.Modules.Add(weatherModule.ModuleName, weatherModule);

        InputOutputFile file = new("Grammar3");
        StreamReader reader = new(Path.Combine(file.PrefixPath, "grammar.yaml"));
        _katheryneChatRobotFactory.SetGrammar(reader.ReadToEnd());

        Assert.Throws<GrammarException>(
            () => ValidateOutput(_katheryneChatRobotFactory.GetRobot(), file));
    }

    [Fact]
    public void KatheryneRobotTest4()
    {
        IParamsModule weatherModule = new MockWeatherModule();
        _katheryneChatRobotFactory.Modules.Clear();
        _katheryneChatRobotFactory.Modules.Add(weatherModule.ModuleName, weatherModule);

        InputOutputFile file = new("Grammar4");
        StreamReader reader = new(Path.Combine(file.PrefixPath, "grammar.yaml"));
        _katheryneChatRobotFactory.SetGrammar(reader.ReadToEnd());

        ValidateOutput(_katheryneChatRobotFactory.GetRobot(), file);
    }

    [Fact]
    public void WeatherModuleTest()
    {
        const string grammar =
            """
            robotName: 凯瑟琳
            stages:
              - name: running
                answer: 向着星辰和深渊！欢迎来到冒险家协会。
                transformers:
                  - pattern: .*?天气|气温.*?
                    nextStageName: weather
                  - pattern: .*?
                    nextStageName: running
              
              - name: weather
                answer: 今天璃月港的天气是@weather/text，气温是@weather/temp。
                transformers:
                  - pattern: 
                    nextStageName: running
            beginStageName: running
            """;

        ModuleBase weatherModule = new WeatherModule();

        _katheryneChatRobotFactory.Modules.Clear();
        _katheryneChatRobotFactory.Modules.Add(weatherModule.ModuleName, weatherModule);
        _katheryneChatRobotFactory.SetGrammar(grammar);

        IChatRobot robot = _katheryneChatRobotFactory.GetRobot();
        IEnumerable<string> answers = robot.ChatNext("今天天气怎么样？");

        Assert.Contains(answers, answer =>
            answer == $"今天璃月港的天气是{weatherModule["text"]}，气温是{weatherModule["temp"]}。");

        answers = robot.ChatNext("今天气温是多少度？");
        Assert.Contains(answers, answer =>
            answer == $"今天璃月港的天气是{weatherModule["text"]}，气温是{weatherModule["temp"]}。");
    }

    private static void ValidateOutput(IChatRobot robot, InputOutputFile file)
    {
        foreach (string output in robot.OnChatStart())
        {
            string? except = file.Output.ReadLine();
            Assert.NotNull(except);
            Assert.Equal(except, output);
        }

        while (file.Input.Peek() >= 0)
        {
            string? input = file.Input.ReadLine();
            Assert.NotNull(input);

            foreach (string output in robot.ChatNext(input))
            {
                string? except = file.Output.ReadLine();
                Assert.NotNull(except);
                Assert.Equal(except, output);
            }
        }

        foreach (string output in robot.OnChatStop())
        {
            string? except = file.Output.ReadLine();
            Assert.NotNull(except);
            Assert.Equal(except, output);
        }
    }

    private class InputOutputFile
    {
        public StreamReader Input { get; }

        public StreamReader Output { get; }

        public string PrefixPath { get; }

        public InputOutputFile(string testName)
        {
            PrefixPath = Path.Combine(Environment.CurrentDirectory, testName);

            Input = new StreamReader(Path.Combine(PrefixPath, "in.txt"));
            Output = new StreamReader(Path.Combine(PrefixPath, "out.txt"));
        }
    }
}