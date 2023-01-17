using Kindxt.Kind;
using Kindxt.Processes;

namespace Kindxt.Charts.MongoDb;

public class MongoDbChart : HelmChartBase, IHelmChart
{
    public MongoDbChart(HelmProcess helmProcess) : base(helmProcess) { }

    public string[] Parameters => new[] { "--mongodb" };
    public string Description => "Install mongodb chart on kind";
    public ExtraPortMapping[] GetPortMapping() => Ports.MongoDb;

    public void Install()
    {
        var configDirectory = Path.Combine("Charts", "MongoDb", "config.yaml");

        base.InstallFromRepo(
            "bitnami/mongodb",
            "bitnami",
            "https://charts.bitnami.com/bitnami",
            "mongodb",
            configDirectory);
    }
}
