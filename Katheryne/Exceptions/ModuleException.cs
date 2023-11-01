namespace Katheryne.Exceptions;

/// <summary>
/// 调用模块中发生的异常
/// </summary>
public class ModuleException : Exception
{
    public ModuleException() : base()
    {

    }

    public ModuleException(string message) : base(message)
    {

    }

    public ModuleException(string message, Exception innerException) : base(message, innerException)
    {

    }
}