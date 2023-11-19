using Katheryne.Abstractions;
using Katheryne.Services;
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

    private void ValidateOutput(IChatRobot robot, InputOutputFile file)
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