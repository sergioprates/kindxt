using Kindxt.Kind;
using Kindxt.Processes;
using System.Text;

namespace Kindxt.Charts.MongoDb;

public class MongoDbChart : HelmChartBase, IHelmChart
{
    public MongoDbChart(HelmProcess helmProcess) : base(helmProcess) { }

    public const string ChartName = "mongodb";
    public string[] Parameters => new[] { "--mongodb" };
    public string Description => "Install mongodb chart on kind";
    public ExtraPortMapping[] GetPortMapping() => Ports.MongoDb;

    public void Install(IReadOnlyList<string> parameters)
    {
        var chartParameters = parameters.Where(x => x.StartsWith(ChartName)).ToList();
        var sb = new StringBuilder();

        //foreach (var parameter in chartParameters)
        //{
        //    var splitParameter  = parameter.Split('=');
        //    var value = splitParameter[1];
        //    var chain = splitParameter[0].Split('.');

        //    var dic = new Dictionary<object, object>();

        //    foreach (var content in chain)
        //    {
        //        dic.Add(content,);
        //    }


        //}
    }

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
