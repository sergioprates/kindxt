using Kindxt.Kind;
using Kindxt.Processes;

namespace Kindxt.Charts.Redis;

public class RedisChart : HelmChartBase, IHelmChart
{
    public RedisChart(HelmProcess helmProcess) : base(helmProcess)
    {
    }

    public void Install()
    {
        var configDirectory = Path.Combine("Charts", "Redis", "config.yaml");

        base.InstallFromRepo("bitnami/redis",
            "bitnami",
            "https://charts.bitnami.com/bitnami",
            "redis",
            configDirectory);
    }

    public string[] Parameters => new[] { "--redis" };
    public string Description => "Install Redis chart on kind";
    public ExtraPortMapping[] GetPortMapping() => Ports.Redis;
}
