using Katheryne.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Katheryne;

public static class ServiceCollectionExtensions
{
    public static void AddYamlDeserializerFactory(this IServiceCollection collection)
    {
        collection.AddSingleton<YamlDeserializerFactory>();
        collection.AddSingleton<DefaultChatRobot>();
        collection.AddScoped<KatheryneChatRobotFactory>();
    }
}