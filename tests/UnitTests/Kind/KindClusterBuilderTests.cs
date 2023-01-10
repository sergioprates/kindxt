using Autofac.Extras.FakeItEasy;
using FakeItEasy;
using Kindxt.Charts;
using Kindxt.Extensions;
using Kindxt.Kind;
using Kindxt.Managers;
using Kindxt.Processes;
using YamlDotNet.Serialization;

namespace UnitTests.Kind;

public class KindClusterBuilderTests : TestBase
{
    [SetUp]
    public void SetUp()
    {
        AutoFake.Provide(A.Fake<FileManager>());
        AutoFake.Provide(A.Fake<HelmChartManager>());
        AutoFake.Provide(A.Fake<KindProcess>());
        AutoFake.Provide(A.Fake<ProcessWrapper>());
    }

    [Test]
    public void ShouldntCreateDirectoryIfParameterToCreateClusterIsNotSpecified()
    {
        AutoFake.Resolve<KindClusterBuilder>().Build(new string[] { "--99" }.ToList());

        A.CallTo(() =>
            AutoFake.Resolve<FileManager>().CreateDirectoryIfNotExists(A<string>.Ignored))
            .MustNotHaveHappened();
    }

    [TestCase("--create-cluster")]
    [TestCase("-c")]
    public void ShouldCreateDirectoryIfContainsParameterCreateCluster(string parameter)
    {
        AutoFake.Resolve<KindClusterBuilder>().Build(new string[] { parameter }.ToList());

        A.CallTo(() =>
            AutoFake.Resolve<FileManager>().CreateDirectoryIfNotExists(A<string>.Ignored))
            .MustHaveHappenedOnceExactly();
    }

    [TestCase("--create-cluster")]
    [TestCase("-c")]
    public void ShouldWriteFileWhenCreateClusterIsSpecified(string parameter)
    {
        var configText = "config";
        var tmpDirectory = Path.Combine(KindxtPath.GetProcessPath(), "tmp", "kind");

        A.CallTo(() => AutoFake.Resolve<ISerializer>().Serialize(A<object>.Ignored)).Returns(configText);
        AutoFake.Resolve<KindClusterBuilder>().Build(new string[] { parameter }.ToList());

        A.CallTo(() =>
            AutoFake.Resolve<FileManager>().WriteFile(Path.Combine(tmpDirectory, "kind-config.yaml"), configText))
            .MustHaveHappenedOnceExactly();
    }

    [TestCase("--create-cluster")]
    [TestCase("-c")]
    public void ShouldExecuteCommandDeleteClusterWhenParameterCreateClusterIsPresent(string parameter)
    {
        AutoFake.Resolve<KindClusterBuilder>().Build(new string[] { parameter }.ToList());

        A.CallTo(() =>
            AutoFake.Resolve<KindProcess>().ExecuteCommand("delete cluster", "", true, A<int>.Ignored))
            .MustHaveHappenedOnceExactly();
    }

    [TestCase("--create-cluster")]
    [TestCase("-c")]
    public void ShouldExecuteCommandCreateCluster(string parameter)
    {
        var tmpDirectory = Path.Combine(KindxtPath.GetProcessPath(), "tmp", "kind");

        A.CallTo(() => AutoFake.Resolve<KindProcess>()
                .ExecuteCommand(A<string>.Ignored, A<string>.Ignored, A<bool>.Ignored, A<int>.Ignored))
                .Returns(AutoFake.Resolve<KindProcess>());

        AutoFake.Resolve<KindClusterBuilder>().Build(new string[] { parameter }.ToList());

        A.CallTo(() =>
            AutoFake.Resolve<KindProcess>()
                .ExecuteCommand("create cluster --config=kind-config.yaml -v 1", tmpDirectory, false, A<int>.Ignored))
            .MustHaveHappenedOnceExactly();
    }

    [Test]
    public void ShouldAddPortMappingFromIdentifiedHelmCharts()
    {
        var helmChart = A.Fake<IHelmChart>();
        var kindConfig = A.Fake<KindConfig>();
        var portMapping = A.Fake<ExtraPortMapping>();
        var listPortMapping = new ExtraPortMapping[] { portMapping };

        A.CallTo(() =>
                AutoFake.Resolve<IDeserializer>().Deserialize<KindConfig>(A<TextReader>.Ignored))
            .Returns(kindConfig);

        A.CallTo(() => helmChart.GetPortMapping())
            .Returns(listPortMapping);

        A.CallTo(() =>
                AutoFake.Resolve<HelmChartManager>().GetHelmCharts(A<List<string>>.Ignored))
            .Returns(new List<IHelmChart>() { helmChart });

        AutoFake.Resolve<KindClusterBuilder>().Build(new string[] { "123" }.ToList());

        A.CallTo(() => kindConfig.AddPortsRange(listPortMapping))
            .MustHaveHappenedOnceExactly();
    }

    [Test]
    public void ShouldInstallHelmChartsIdentifiedByParameters()
    {
        var helmChart = A.Fake<IHelmChart>();
        var kindConfig = A.Fake<KindConfig>();

        A.CallTo(() =>
                AutoFake.Resolve<IDeserializer>().Deserialize<KindConfig>(A<TextReader>.Ignored))
            .Returns(kindConfig);

        A.CallTo(() =>
                AutoFake.Resolve<HelmChartManager>().GetHelmCharts(A<List<string>>.Ignored))
            .Returns(new List<IHelmChart>() { helmChart });

        AutoFake.Resolve<KindClusterBuilder>().Build(new string[] { "123" }.ToList());

        A.CallTo(() => helmChart.Install())
            .MustHaveHappenedOnceExactly();
    }
}
