using Kindxt.Charts.Adminer;
using Kindxt.Charts.Citus;
using Kindxt.Charts.Istio;
using Kindxt.Charts.Keda;
using Kindxt.Charts.MongoDb;
using Kindxt.Charts.NginxIngress;
using Kindxt.Charts.Postgres;
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
        services.AddSingleton(new SerializerBuilder()
        .WithNamingConvention(CamelCaseNamingConvention.Instance)
        .Build());

        services.AddSingleton(new DeserializerBuilder()
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
        services.AddSingleton<KubectlProcess>();
        return services;
    }

    public static IServiceCollection AddCharts(this IServiceCollection services)
    {
        services.AddSingleton<SqlServerChart>();
        services.AddSingleton<MongoDbChart>();
        services.AddSingleton<PostgresChart>();
        services.AddSingleton<NginxIngressChart>();
        services.AddSingleton<PgAdminChart>();
        services.AddSingleton<IstioChart>();
        services.AddSingleton<KedaChart>();
        services.AddSingleton<CitusChart>();
        return services;
    }

    public static IServiceCollection AddManagers(this IServiceCollection services)
    {
        services.AddSingleton<FileManager>();
        services.AddSingleton<HelmChartManager>();
        return services;
    }
}
