using System.Text.RegularExpressions;

namespace Katheryne.Models;

/// <summary>
/// 格式化字符串模型类
/// </summary>
public class StringFormatter
{
    private readonly string _originString;
    private readonly List<FormatTag> _formatTags = new();

    public StringFormatter(string originString)
    {
        _originString = originString;
        
        GetFormatTags();
    }

    public bool IsFormat => _formatTags.Count != 0;

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

        return result;
    }

    private void GetFormatTags()
    {
        List<int> indexes = new();

        for (var i = 0; i < _originString.Length; i++)
        {
            if (_originString[i] == '$')
            {
                indexes.Add(i);
            }
        }

        foreach (int index in indexes)
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
    }
}