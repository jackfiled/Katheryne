using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Katheryne.Services;

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