using Kindxt.Charts.SqlServer;
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


        services.AddProcesses();
        services.AddCharts();
        services.AddManagers();


        return services;
    }
    public static IServiceCollection AddProcesses(this IServiceCollection services)
    {
        services.AddSingleton<KindProcess>();
        services.AddSingleton<HelmProcess>();
        return services;
    }

    public static IServiceCollection AddCharts(this IServiceCollection services)
    {
        services.AddSingleton<SqlServerChart>();
        return services;
    }
    public static IServiceCollection AddManagers(this IServiceCollection services)
    {
        services.AddSingleton<FileManager>();
        services.AddSingleton<HelmChartManager>();
        return services;
    }
}
