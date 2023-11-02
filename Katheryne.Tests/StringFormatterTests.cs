using System.Text.RegularExpressions;
using Katheryne.Abstractions;
using Katheryne.Exceptions;
using Katheryne.Models;
using Katheryne.Tests.Mocks;

namespace Katheryne.Tests;

public class StringFormatterTests
{
    private readonly Dictionary<string, IParamsModule> _modules = new();

    public StringFormatterTests()
    {
        var module = new MockParamModule();
        _modules.Add(module.ModuleName, module);
    }

    [Fact]
    public void FormatTest1()
    {
        StringFormatter formatter = new("Hello $1", _modules);

        Regex regex = new("I'm (.*)");

        Match match = regex.Match("I'm jackfiled");
        
        Assert.True(match.Success);
        Assert.True(formatter.IsFormat);
        
        Assert.Equal("Hello jackfiled", formatter.Format(match.Groups));
    }

    [Fact]
    public void FormatTest2()
    {
        StringFormatter formatter = new("$你好呀 $1, 欢迎你离开垃圾的$2.", _modules);

        Regex regex = new("你好，我是(.*)，我来自(.*)\\.");

        Match match = regex.Match("你好，我是Ichirinko，我来自Trinity.");
        
        Assert.True(match.Success);
        Assert.True(formatter.IsFormat);
        
        Assert.Equal("$你好呀 Ichirinko, 欢迎你离开垃圾的Trinity.", formatter.Format(match.Groups));
    }

    [Fact]
    public void ParamFormatTest1()
    {
        StringFormatter formatter = new("Test @test/hello", _modules);

        Regex regex = new(".*?");
        Match match = regex.Match("Test Input");

        Assert.True(formatter.IsFormat);
        Assert.Equal("Test Hello, Katheryne", formatter.Format(match.Groups));
    }

    [Fact]
    public void ParamFormatTest2()
    {
        Assert.Throws<GrammarException>(() => new StringFormatter("Test @test/nonexistent", _modules));
    }
}