using System.Text.RegularExpressions;
using Katheryne.Models;

namespace Katheryne.Tests;

public class StringFormatterTests
{
    [Fact]
    public void FormatTest1()
    {
        StringFormatter formatter = new("Hello $1");

        Regex regex = new("I'm (.*)");

        Match match = regex.Match("I'm jackfiled");
        
        Assert.True(match.Success);
        Assert.True(formatter.IsFormat);
        
        Assert.Equal("Hello jackfiled", formatter.Format(match.Groups));
    }

    [Fact]
    public void FormatTest2()
    {
        StringFormatter formatter = new("$你好呀 $1, 欢迎你离开垃圾的$2.");

        Regex regex = new("你好，我是(.*)，我来自(.*)\\.");

        Match match = regex.Match("你好，我是Ichirinko，我来自Trinity.");
        
        Assert.True(match.Success);
        Assert.True(formatter.IsFormat);
        
        Assert.Equal("$你好呀 Ichirinko, 欢迎你离开垃圾的Trinity.", formatter.Format(match.Groups));
    }
}