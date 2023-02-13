using FakeItEasy;
using FluentAssertions;
using Kindxt.Charts.Postgres;
using Kindxt.Extensions;
using Kindxt.Kind;
using Kindxt.Processes;

namespace UnitTests.Charts.Postgres;

public class PostgresChartTests : TestBase
{
    private const string ReleaseName = "postgres";
    private const string Namespace = "default";
    private const string ChartName = "bitnami/postgresql";
    private const string RepoName = "bitnami";
    private const string RepoUrl = "https://charts.bitnami.com/bitnami";
    private string _pathConfig = Path.Combine("Charts", "Postgres", "config.yaml");

    [SetUp]
    public void SetUp()
    {
        AutoFake.Provide(A.Fake<HelmProcess>());
        A.CallTo(() => AutoFake.Resolve<HelmProcess>().ExecuteCommand(A<string>.Ignored,
                A<string>.Ignored, A<bool>.Ignored, A<int>.Ignored))
            .Returns(AutoFake.Resolve<HelmProcess>());
    }
    [Test]
    public void ParametersShouldBeCorrect()
    {
        AutoFake.Resolve<PostgresChart>().Parameters
            .Should().BeEquivalentTo(new string[] { "--postgres", "-pssql" });
    }
    [Test]
    public void DescriptionShouldBeCorrect()
    {
        AutoFake.Resolve<PostgresChart>().Description
            .Should().Be("Install postgres chart on kind");
    }

    [Test]
    public void GetPortMappingShouldReturnCorrectMap()
    {
        AutoFake.Resolve<PostgresChart>().GetPortMapping()
            .Should().BeEquivalentTo(new ExtraPortMapping[]
            {
                new()
                {
                    ContainerPort = 30001,
                    HostPort = 5432,
                    Protocol = "TCP"
                }
            });
    }

    [Test]
    public void InstallShouldExecuteCommandRepoAdd()
    {
        AutoFake.Resolve<PostgresChart>().Install();

        A.CallTo(() => AutoFake.Resolve<HelmProcess>().ExecuteCommand(
                $"repo add {RepoName} {RepoUrl}", A<string>.Ignored,
                true, A<int>.Ignored))
            .MustHaveHappenedOnceExactly();
    }
    [Test]
    public void InstallShouldExecuteCommandRepoUpdate()
    {
        AutoFake.Resolve<PostgresChart>().Install();

        A.CallTo(() => AutoFake.Resolve<HelmProcess>().ExecuteCommand(
                "repo update", A<string>.Ignored,
                true, A<int>.Ignored))
            .MustHaveHappenedOnceExactly();
    }
    [Test]
    public void InstallShouldExecuteCommandUninstall()
    {
        AutoFake.Resolve<PostgresChart>().Install();

        A.CallTo(() => AutoFake.Resolve<HelmProcess>().ExecuteCommand(
                $"uninstall {ReleaseName} -n {Namespace}", A<string>.Ignored,
                true, A<int>.Ignored))
            .MustHaveHappenedOnceExactly();
    }
    [Test]
    public void InstallShouldExecuteCommandInstall()
    {
        AutoFake.Resolve<PostgresChart>().Install();

        A.CallTo(() => AutoFake.Resolve<HelmProcess>().ExecuteCommandWithWait(
                $"install {ReleaseName} {ChartName} -n {Namespace} --wait --debug --timeout=5m -f {_pathConfig}",
                KindxtPath.GetProcessPath(),
                false, A<int>.Ignored))
            .MustHaveHappenedOnceExactly();
    }
}
