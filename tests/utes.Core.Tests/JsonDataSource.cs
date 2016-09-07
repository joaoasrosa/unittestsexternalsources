using Xunit;
using utes.Xunit;

namespace utes.Core.Tests
{
    public class JsonDataSource
    {
        [Theory]
        [JsonDataSource(ResourceName = @"test", ResourceType = typeof(Resources))]
        public void DummyTest(int a)
        {
            Assert.Equal(1, a);
        }
    }
}
