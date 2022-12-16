using Kindxt.Kind;
using Kindxt.Managers;
using Kindxt.Processes;
using Microsoft.Extensions.DependencyInjection;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Kindxt.Extensions;
public static class ServiceCollectionExtensions
{
    public static IServiceCollection RegisterDependencies(this IServiceCollection services)
    {
        services.AddSingleton<ISerializer>(new SerializerBuilder()
        .WithNamingConvention(CamelCaseNamingConvention.Instance)
        .Build());

        services.AddSingleton<IDeserializer>(new DeserializerBuilder()
        .WithNamingConvention(CamelCaseNamingConvention.Instance)
        .Build());

        services.AddSingleton<KindClusterBuilder>();
        services.AddSingleton<KindProcess>();

        services.AddSingleton<FileManager>();
        services.AddSingleton<HelmChartManager>();

        return services;
    }
}
