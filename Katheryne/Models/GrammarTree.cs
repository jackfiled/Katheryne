using System.Text.RegularExpressions;
using Katheryne.Exceptions;

namespace Katheryne.Models;

public class GrammarTree
{
    private readonly Dictionary<string, InnerStage> _stages = new();

    public GrammarTree(LexicalModel model)
    {
        foreach (Stage stage in model.Stages)
        {
            _stages[stage.Name] = new InnerStage(stage);
        }

        if (!Validate(model))
        {
            throw new GrammarException("使用了未声明的阶段名");
        }
    }

    /// <summary>
    /// 获得下一个阶段
    /// </summary>
    /// <param name="currentStage">当前所在阶段</param>
    /// <param name="input">用户输入</param>
    /// <returns>元组，第一个参数是下一个阶段名称 第二个参数是机器人回答</returns>
    /// <exception cref="GrammarException"></exception>
    public (string, string) NextStage(string currentStage, string input)
    {
        List<InnerTransformer> transformers = _stages[currentStage].Transformers;

        foreach (InnerTransformer transformer in transformers)
        {
            Match match = transformer.Pattern.Match(input);

            if (match.Success)
            {
                return (_stages[transformer.NextStage].Name,
                    _stages[transformer.NextStage].Answer);
            }
        }

        throw new GrammarException("Failed to get next stage.");
    }

    public string this[string index] => _stages[index].Answer;

    /// <summary>
    /// 主要验证语法的两个特点
    /// 1. 起始阶段是否被定义
    /// 2. 每个Transformer的下一个阶段是否被定义
    /// </summary>
    /// <returns></returns>
    private bool Validate(LexicalModel model)
    {
        if (!_stages.ContainsKey(model.BeginStageName))
        {
            return false;
        }

        return model.Stages.All((stage) =>
        {
            return stage.Transformers.All(t => _stages.ContainsKey(t.NextStageName));
        });
    }
    
    private class InnerStage
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
    
    private class InnerTransformer
    {
        public Regex Pattern { get; }
        
        public string NextStage { get; }

        public InnerTransformer(Transformer transformer)
        {
            NextStage = transformer.NextStageName;

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
}