namespace Katheryne.Models;

internal class InnerStage
{
    public string Name { get; }

    public List<InnerTransformer> Transformers { get; } = new();
        
    public StringFormatter Answer { get; }

    public InnerStage(Stage stage)
    {
        Name = stage.Name;
        Answer = new StringFormatter(stage.Answer);

        foreach (Transformer transformer in stage.Transformers)
        {
            Transformers.Add(new InnerTransformer(transformer));
        }
    }
}