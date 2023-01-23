using Autofac.Extras.FakeItEasy;
using FakeItEasy;
using FluentAssertions;
using Kindxt.Charts.Redis;
using Kindxt.Extensions;
using Kindxt.Kind;
using Kindxt.Processes;

namespace UnitTests.Charts.Redis;

internal class RedisChartTests : TestBase
{
    private const string ReleaseName = "redis";
    private const string Namespace = "default";
    private const string ChartName = "bitnami/redis";
    private const string RepoName = "bitnami";
    private const string RepoUrl = "https://charts.bitnami.com/bitnami";
    private string _pathConfig = Path.Combine("Charts", "Redis", "config.yaml");

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
        AutoFake.Resolve<RedisChart>().Parameters
            .Should().BeEquivalentTo(new string[] { "--redis" });
    }

    [Test]
    public void DescriptionShouldBeCorrect()
    {
        AutoFake.Resolve<RedisChart>().Description
            .Should().Be("Install Redis chart on kind");
    }

    [Test]
    public void GetPortMappingShouldReturnCorrectMap()
    {
        AutoFake.Resolve<RedisChart>().GetPortMapping()
            .Should().BeEquivalentTo(new ExtraPortMapping[]
            {
                new()
                {
                    ContainerPort = 30011,
                    HostPort = 6379,
                    Protocol = "TCP"
                }
            });
    }

    [Test]
    public void InstallShouldExecuteCommandRepoAdd()
    {
        AutoFake.Resolve<RedisChart>().Install();

        A.CallTo(() => AutoFake.Resolve<HelmProcess>().ExecuteCommand(
                $"repo add {RepoName} {RepoUrl}", A<string>.Ignored,
                true, A<int>.Ignored))
            .MustHaveHappenedOnceExactly();
    }

    [Test]
    public void InstallShouldExecuteCommandRepoUpdate()
    {
        AutoFake.Resolve<RedisChart>().Install();

        A.CallTo(() => AutoFake.Resolve<HelmProcess>().ExecuteCommand(
                "repo update", A<string>.Ignored,
                true, A<int>.Ignored))
            .MustHaveHappenedOnceExactly();
    }

    [Test]
    public void InstallShouldExecuteCommandUninstall()
    {
        AutoFake.Resolve<RedisChart>().Install();

        A.CallTo(() => AutoFake.Resolve<HelmProcess>().ExecuteCommand(
                $"uninstall {ReleaseName} -n {Namespace}", A<string>.Ignored,
                true, A<int>.Ignored))
            .MustHaveHappenedOnceExactly();
    }

    [Test]
    public void InstallShouldExecuteCommandInstall()
    {
        AutoFake.Resolve<RedisChart>().Install();

        A.CallTo(() => AutoFake.Resolve<HelmProcess>().ExecuteCommand(
                $"install {ReleaseName} {ChartName} -n {Namespace} --wait --debug --timeout=5m -f {_pathConfig}",
                KindxtPath.GetProcessPath(),
                false, A<int>.Ignored))
            .MustHaveHappenedOnceExactly();
    }
}
