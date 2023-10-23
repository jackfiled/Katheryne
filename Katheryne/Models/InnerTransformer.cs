using System.Text.RegularExpressions;
using Katheryne.Exceptions;

namespace Katheryne.Models;

internal class InnerTransformer
{
    public Regex? Pattern { get; }
        
    public string RowPattern { get; }
        
    public string NextStage { get; }

    public InnerTransformer(Transformer transformer)
    {
        NextStage = transformer.NextStageName;
        RowPattern = transformer.Pattern;

        if (string.IsNullOrEmpty(RowPattern))
        {
            return;
        }

        try
        {
            Pattern = new Regex(transformer.Pattern);
        }
        catch (ArgumentException e)
        {
            throw new GrammarException($"Failed to Parse regex:{transformer.Pattern}.", e);
        }
    }
}