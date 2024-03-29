using Katheryne.Models;
using Katheryne.Services;
using YamlDotNet.Serialization;

namespace Katheryne.Tests;

public class YamlDeserializerTests
{
    private readonly YamlDeserializerFactory _factory = new();
    
    [Fact]
    public void DeserializerTest1()
    {
        const string document =
            """
            robotName: 凯瑟琳
            stages:
              - name: start
                answer: 向着星辰和深渊！欢迎来到冒险家协会。
                transformers:
                  - pattern: .*?
                    nextStageName: running
                
              - name: running
                answer: 对不起，做不到。
                transformers:
                  - pattern: .*?
                    nextStageName: running
            beginStageName: start
            """;
        
        IDeserializer deserializer = _factory.GetDeserializer();
        LexicalModel actual = deserializer.Deserialize<LexicalModel>(document);
        
        Assert.Equal("凯瑟琳", actual.RobotName);
        Assert.Equal("start", actual.BeginStageName);
        
        Assert.Contains(actual.Stages, s => s.Name == "start");
        Assert.Contains(actual.Stages, s => s.Name == "running");

        Stage stage = actual.Stages[1];
        Assert.Equal("running", stage.Name);
        Assert.Contains(stage.Transformers, t => t.NextStageName == "running");
    }

    [Fact]
    public void DeserializerTest2()
    {
        const string document =
            """
            robotName: 凯瑟琳
            stages:
              - name: start
                answer: 向着星辰和深渊！欢迎来到冒险家协会。
                transformers:
                  - pattern: 
                    nextStageName: running
                  - pattern: .*?
                    nextStageName: running
            beginStageName: start
            """;
        
        IDeserializer deserializer = _factory.GetDeserializer();
        LexicalModel actual = deserializer.Deserialize<LexicalModel>(document);

        Assert.Contains(actual.Stages, s => s.Name == "start");

        Stage stage = actual.Stages[0];

        Assert.Contains(stage.Transformers, s => string.IsNullOrEmpty(s.Pattern));
        Assert.Contains(stage.Transformers, s => s.Pattern == ".*?");
    }
    
}