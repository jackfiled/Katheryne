namespace Katheryne.Models;

internal class InnerStage
{
    public string Name { get; }

    public List<InnerTransformer> Transformers { get; } = new();
        
    public string Answer { get; }

    public InnerStage(Stage stage)
    {
        Name = stage.Name;
        Answer = stage.Answer;

        foreach (Transformer transformer in stage.Transformers)
        {
            Transformers.Add(new InnerTransformer(transformer));
        }
    }
}