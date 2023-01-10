using FakeItEasy;
using FluentAssertions;
using Kindxt.Charts.Adminer;
using Kindxt.Extensions;
using Kindxt.Kind;
using Kindxt.Processes;

namespace UnitTests.Charts.PgAdmin;

public class PgAdminChartTests : TestBase
{
    private const string ReleaseName = "pgadmin";
    private const string Namespace = "default";
    private const string ChartName = "runix/pgadmin4";
    private const string RepoName = "runix";
    private const string RepoUrl = "https://helm.runix.net";
    private string _pathConfig = Path.Combine("Charts", "PgAdmin", "config.yaml");

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
        AutoFake.Resolve<PgAdminChart>().Parameters
            .Should().BeEquivalentTo(new string[] { "--pg-admin", "-pssql-admin" });
    }
    [Test]
    public void DescriptionShouldBeCorrect()
    {
        AutoFake.Resolve<PgAdminChart>().Description
            .Should().Be("Install pgadmin chart on kind");
    }

    [Test]
    public void GetPortMappingShouldReturnCorrectMap()
    {
        AutoFake.Resolve<PgAdminChart>().GetPortMapping()
            .Should().BeEquivalentTo(new ExtraPortMapping[]
            {
                new()
                {
                    ContainerPort = 30002,
                    HostPort = 9000,
                    Protocol = "TCP"
                }
            });
    }

    [Test]
    public void InstallShouldExecuteCommandRepoAdd()
    {
        AutoFake.Resolve<PgAdminChart>().Install();

        A.CallTo(() => AutoFake.Resolve<HelmProcess>().ExecuteCommand(
                $"repo add {RepoName} {RepoUrl}", A<string>.Ignored,
                true, A<int>.Ignored))
            .MustHaveHappenedOnceExactly();
    }
    [Test]
    public void InstallShouldExecuteCommandRepoUpdate()
    {
        AutoFake.Resolve<PgAdminChart>().Install();

        A.CallTo(() => AutoFake.Resolve<HelmProcess>().ExecuteCommand(
                "repo update", A<string>.Ignored,
                true, A<int>.Ignored))
            .MustHaveHappenedOnceExactly();
    }
    [Test]
    public void InstallShouldExecuteCommandUninstall()
    {
        AutoFake.Resolve<PgAdminChart>().Install();

        A.CallTo(() => AutoFake.Resolve<HelmProcess>().ExecuteCommand(
                $"uninstall {ReleaseName} -n {Namespace}", A<string>.Ignored,
                true, A<int>.Ignored))
            .MustHaveHappenedOnceExactly();
    }
    [Test]
    public void InstallShouldExecuteCommandInstall()
    {
        AutoFake.Resolve<PgAdminChart>().Install();

        A.CallTo(() => AutoFake.Resolve<HelmProcess>().ExecuteCommand(
                $"install {ReleaseName} {ChartName} -n {Namespace} --wait --debug --timeout=5m -f {_pathConfig}",
                KindxtPath.GetProcessPath(),
                false, A<int>.Ignored))
            .MustHaveHappenedOnceExactly();
    }
}
