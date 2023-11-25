using Katheryne.Abstractions;
using Katheryne.Modules;
using Katheryne.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Katheryne;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// 在服务集合中添加Katheryne DSL解释机器人服务
    /// </summary>
    public static void AddKatheryne(this IServiceCollection collection)
    {
        collection.AddSingleton<YamlDeserializerFactory>();
        collection.AddSingleton<DefaultChatRobot>();
        collection.AddSingleton<WeatherModule>();

        collection.AddSingleton<IChatRobotFactory, KatheryneChatRobotFactory>(serviceProvider =>
        {
            var factory = new KatheryneChatRobotFactory(
                serviceProvider.GetRequiredService<YamlDeserializerFactory>(),
                serviceProvider.GetRequiredService<ILogger<KatheryneChatRobotFactory>>(),
                serviceProvider.GetRequiredService<ILogger<KatheryneChatRobot>>(),
                serviceProvider.GetRequiredService<DefaultChatRobot>());

            WeatherModule module = serviceProvider.GetRequiredService<WeatherModule>();

            factory.Modules.Add(module.ModuleName, module);

            return factory;
        });
    }
}