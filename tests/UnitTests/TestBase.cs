using Autofac.Extras.FakeItEasy;

namespace UnitTests
{
    public class TestBase
    {
        protected AutoFake AutoFake;

        [SetUp]
        public void SetUp()
        {
            AutoFake = new AutoFake();
        }
    }
}
