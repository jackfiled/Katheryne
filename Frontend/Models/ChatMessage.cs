namespace Frontend.Models;

public class ChatMessage
{
    public required string Sender { get; init; }
        
    public required string Text { get; init; }
        
    public required bool Left { get; init; }
}