namespace Katheryne.Models;

public class LexicalModel
{
    public required string RobotName { get; set; }
    
    public required List<Stage> Stages { get; set; }
    
    public required string BeginStageName { get; set; }
}