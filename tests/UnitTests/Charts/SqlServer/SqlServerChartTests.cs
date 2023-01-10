using FakeItEasy;
using FluentAssertions;
using Kindxt.Charts.SqlServer;
using Kindxt.Extensions;
using Kindxt.Kind;
using Kindxt.Processes;

namespace UnitTests.Charts.SqlServer;

public class SqlServerChartTests : TestBase
{
    private const string ReleaseName = "sqlserver";
    private const string Namespace = "default";
    private const string ChartName = "stable/mssql-linux";
    private const string RepoUrl = "https://charts.helm.sh/stable";
    private string _pathConfig = Path.Combine("Charts", "SqlServer", "config.yaml");
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
        AutoFake.Resolve<SqlServerChart>().Parameters
            .Should().BeEquivalentTo(new string[] { "--sqlserver", "-sql" });
    }
    [Test]
    public void DescriptionShouldBeCorrect()
    {
        AutoFake.Resolve<SqlServerChart>().Description
            .Should().Be("Install sqlserver chart on kind");
    }

    [Test]
    public void GetPortMappingShouldReturnCorrectMap()
    {
        AutoFake.Resolve<SqlServerChart>().GetPortMapping()
            .Should().BeEquivalentTo(new ExtraPortMapping[]
            {
                new()
                {
                    ContainerPort = 30000,
                    HostPort = 1433,
                    Protocol = "TCP"
                }
            });
    }

    [Test]
    public void InstallShouldExecuteCommandRepoAdd()
    {
        AutoFake.Resolve<SqlServerChart>().Install();

        A.CallTo(() => AutoFake.Resolve<HelmProcess>().ExecuteCommand(
                $"repo add stable {RepoUrl}", A<string>.Ignored,
                true, A<int>.Ignored))
            .MustHaveHappenedOnceExactly();
    }
    [Test]
    public void InstallShouldExecuteCommandRepoUpdate()
    {
        AutoFake.Resolve<SqlServerChart>().Install();

        A.CallTo(() => AutoFake.Resolve<HelmProcess>().ExecuteCommand(
                "repo update", A<string>.Ignored,
                true, A<int>.Ignored))
            .MustHaveHappenedOnceExactly();
    }
    [Test]
    public void InstallShouldExecuteCommandUninstall()
    {
        AutoFake.Resolve<SqlServerChart>().Install();

        A.CallTo(() => AutoFake.Resolve<HelmProcess>().ExecuteCommand(
                $"uninstall {ReleaseName} -n {Namespace}", A<string>.Ignored,
                true, A<int>.Ignored))
            .MustHaveHappenedOnceExactly();
    }
    [Test]
    public void InstallShouldExecuteCommandInstall()
    {
        AutoFake.Resolve<SqlServerChart>().Install();

        A.CallTo(() => AutoFake.Resolve<HelmProcess>().ExecuteCommand(
                $"install {ReleaseName} {ChartName} -n {Namespace} --wait --debug --timeout=5m -f {_pathConfig}",
                KindxtPath.GetProcessPath(),
                false, A<int>.Ignored))
            .MustHaveHappenedOnceExactly();
    }
}
