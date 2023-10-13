namespace Katheryne.Exceptions;

/// <summary>
/// 语法中存在的错误
/// </summary>
public class GrammarException : Exception
{
    public GrammarException() : base()
    {
        
    }

    public GrammarException(string message) : base(message)
    {
        
    }

    public GrammarException(string message, Exception innerException) : base(message, innerException)
    {
        
    }
}