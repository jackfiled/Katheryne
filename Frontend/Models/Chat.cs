using Katheryne.Abstractions;

namespace Frontend.Models;

public class Chat
{
    public Guid Guid { get; } = Guid.NewGuid();
    
    public required IChatRobot Robot { get; init; }

    public string Title { get; set; } = string.Empty;
    
    public bool Selected { get; set; }

    public List<ChatMessage> Messages { get; } = new();
}