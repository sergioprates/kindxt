using Autofac.Extras.FakeItEasy;
using FakeItEasy;
using Kindxt.Extensions;
using Kindxt.Kind;
using Kindxt.Managers;
using Kindxt.Processes;
using YamlDotNet.Serialization;

namespace UnitTests.Kind
{
    public class KindClusterBuilderTests : TestBase
    {
        [SetUp]
        public void SetUp()
        {
            AutoFake.Provide(A.Fake<FileManager>());
            AutoFake.Provide(A.Fake<HelmChartManager>());
            AutoFake.Provide(A.Fake<KindProcess>());
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
            Assert.Fail("Escrever depois, precisa ver como vai ficar o caminho dos arquivos");
        }
    }
}
