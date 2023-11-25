namespace Katheryne.Models;

/// <summary>
/// 词法模型
/// </summary>
public class LexicalModel
{
    public required string RobotName { get; set; }
    
    public required List<Stage> Stages { get; set; }
    
    public required string BeginStageName { get; set; }
}