using FakeItEasy;
using FluentAssertions;
using Kindxt.Charts.MongoDb;
using Kindxt.Extensions;
using Kindxt.Kind;
using Kindxt.Processes;

namespace UnitTests.Charts.MongoDb
{
    public class MongoDbChartTest : TestBase
    {
        private const string ReleaseName = "mongodb";
        private const string Namespace = "default";
        private const string ChartName = "bitnami/mongodb";
        private const string RepoUrl = "https://charts.bitnami.com/bitnami";
        private string _pathConfig = Path.Combine("Charts", "MongoDb", "config.yaml");

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
            AutoFake.Resolve<MongoDbChart>().Parameters
                .Should().BeEquivalentTo(new string[] { "--mongodb" });
        }

        [Test]
        public void DescriptionShouldBeCorrect()
        {
            AutoFake.Resolve<MongoDbChart>().Description
                .Should().Be("Install mongodb chart on kind");
        }

        [Test]
        public void GetPortMappingShouldReturnCorrectMap()
        {
            AutoFake.Resolve<MongoDbChart>().GetPortMapping()
                .Should().BeEquivalentTo(new ExtraPortMapping[]
                {
                    new()
                    {
                        ContainerPort = 30008,
                        HostPort = 27017,
                        Protocol = "TCP"
                    }
                });
        }

        [Test]
        public void InstallShouldExecuteCommandRepoAdd()
        {
            AutoFake.Resolve<MongoDbChart>().Install();

            A.CallTo(() => AutoFake.Resolve<HelmProcess>().ExecuteCommand(
                    $"repo add bitnami {RepoUrl}", A<string>.Ignored,
                    true, A<int>.Ignored))
                .MustHaveHappenedOnceExactly();
        }

        [Test]
        public void InstallShouldExecuteCommandRepoUpdate()
        {
            AutoFake.Resolve<MongoDbChart>().Install();

            A.CallTo(() => AutoFake.Resolve<HelmProcess>().ExecuteCommand(
                    "repo update", A<string>.Ignored,
                    true, A<int>.Ignored))
                .MustHaveHappenedOnceExactly();
        }

        [Test]
        public void InstallShouldExecuteCommandUninstall()
        {
            AutoFake.Resolve<MongoDbChart>().Install();

            A.CallTo(() => AutoFake.Resolve<HelmProcess>().ExecuteCommand(
                    $"uninstall {ReleaseName} -n {Namespace}", A<string>.Ignored,
                    true, A<int>.Ignored))
                .MustHaveHappenedOnceExactly();
        }

        [Test]
        public void InstallShouldExecuteCommandInstall()
        {
            AutoFake.Resolve<MongoDbChart>().Install();

            A.CallTo(() => AutoFake.Resolve<HelmProcess>().ExecuteCommand(
                    $"install {ReleaseName} {ChartName} -n {Namespace} --wait --debug --timeout=5m -f {_pathConfig}",
                    KindxtPath.GetProcessPath(),
                    false, A<int>.Ignored))
                .MustHaveHappenedOnceExactly();
        }
    }
}
