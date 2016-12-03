using Xunit;
using utes.Xunit;

namespace utes.Core.Tests
{
    public class Foo
    {
        public string Bar { get; set; }

        public int Dummy { get; set; }
    }

    public class JsonDataSource
    {
        [Theory]
        [JsonDataSource(ResourceName = @"test", ResourceType = typeof(Resources))]
        public void DummyTest(int a)
        {
            Assert.Equal(1, a);
        }

        [Theory]
        [JsonDataSource(ResourceName = @"test", ResourceType = typeof(Resources))]
        public void Dummy1Test(int a, Foo foo)
        {
            Assert.Equal(1, a);
        }
    }
}
