using System.Text.RegularExpressions;
using Katheryne.Abstractions;
using Katheryne.Exceptions;

namespace Katheryne.Models;

/// <summary>
/// 格式化字符串模型类
/// </summary>
public class StringFormatter
{
    private static readonly HashSet<char> s_delimiters = new()
    {
        ',', '.', '!', ';', '?', ' ', '，', '。', '；'
    };

    private readonly string _originString;
    private readonly Dictionary<string, IParamsModule> _modules;
    private readonly List<FormatTag> _formatTags = new();
    private readonly List<GrammarParam> _params = new();

    public StringFormatter(string originString, Dictionary<string, IParamsModule> modules)
    {
        _originString = originString;
        _modules = modules;
        
        GetFormatTags();
    }

    public bool IsFormat => _formatTags.Count != 0 || _params.Count != 0;

    public string RowString => _originString;

    public string Format(GroupCollection collection)
    {
        var result = new string(_originString);

        foreach (FormatTag tag in _formatTags)
        {
            if (tag.Index > collection.Count)
            {
                continue;
            }

            result = result.Replace(tag.Value, collection[tag.Index].Value);
        }

        foreach (GrammarParam param in _params)
        {
            result = result.Replace(param.OriginString,
                _modules[param.Module][param.Param]);
        }

        return result;
    }

    private void GetFormatTags()
    {
        List<int> tagIndices = new();
        List<int> paramIndices = new();

        for (var i = 0; i < _originString.Length; i++)
        {
            if (_originString[i] == '$')
            {
                tagIndices.Add(i);
            }
            else if (_originString[i] == '@')
            {
                paramIndices.Add(i);
            }
        }

        foreach (int index in tagIndices)
        {
            string value = string.Empty;
            int pos = index + 1;

            // 如果 $ 之后的字符不是数字就忽略处理
            if (_originString[pos] < '0' || _originString[pos] > '9')
            {
                continue;
            }

            while (_originString[pos] >= '0' && _originString[pos] <= '9')
            {
                value = value + _originString[pos];
                pos++;

                if (pos >= _originString.Length)
                {
                    break;
                }
            }
            
            _formatTags.Add(new FormatTag('$' + value, int.Parse(value)));
        }

        List<char> chars = new();
        foreach (int index in paramIndices)
        {
            chars.Clear();
            int i = index + 1;

            while (i < _originString.Length && !s_delimiters.Contains(_originString[i]))
            {
                chars.Add(_originString[i]);
                i++;
            }

            string text = new(chars.ToArray());

            string[] array = text.Split('/');

            if (array.Length != 2)
            {
                throw new GrammarException($"Failed to parse grammar param: {text}.");
            }

            if (!_modules.ContainsKey(array[0]))
            {
                throw new GrammarException($"Unknown module {array[0]}.");
            }

            if (!_modules[array[0]].ContainsParam(array[1]))
            {
                throw new GrammarException($"Module {array[0]} doesn't support {array[1]}.");
            }

            _params.Add(new GrammarParam(
                '@' + text,
                array[0],
                array[1]
            ));
        }
    }
}