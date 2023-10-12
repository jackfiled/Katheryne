namespace Katheryne.Models;

public class Stage
{
    public required string Name { get; set; }
    
    public required List<Transformer> Transformers { get; set; }
    
    public required string Answer { get; set; }

    public override bool Equals(object? obj)
    {
        if (obj is not Stage other)
        {
            return false;
        }

        return other.Name == Name;
    }

    public override int GetHashCode()
    {
        return Name.GetHashCode();
    }

    public override string ToString()
    {
        return Name;
    }
}