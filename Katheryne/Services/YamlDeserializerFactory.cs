using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Katheryne.Services;

/// <summary>
/// YAML 反序列化对象工厂
/// </summary>
public class YamlDeserializerFactory
{
    private readonly DeserializerBuilder _builder;

    public YamlDeserializerFactory()
    {
        _builder = new DeserializerBuilder();
        _builder.WithNamingConvention(CamelCaseNamingConvention.Instance);
    }
    
    public IDeserializer GetDeserializer()
    {
        return _builder.Build();
    }
}