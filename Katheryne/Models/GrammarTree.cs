using Katheryne.Abstractions;
using Katheryne.Exceptions;

namespace Katheryne.Models;

public class GrammarTree
{
    private readonly Dictionary<string, InnerStage> _stages = new();

    public GrammarTree(LexicalModel model, Dictionary<string, IParamsModule> modules)
    {
        foreach (Stage stage in model.Stages)
        {
            _stages[stage.Name] = new InnerStage(stage, modules);
        }

        if (!Validate(model))
        {
            throw new GrammarException("使用了未声明的阶段名");
        }
    }
    

    internal InnerStage this[string index] => _stages[index];

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
}