using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using utes.Xunit;
using Xunit;

namespace utes.Json.Tests
{
    public class Json
    {
        //[Fact]
        //public void ReadTest()
        //{
        //    // Arrange
        //    var json = new utes.Json.Json();

        //    // Act
        //    var t = typeof(Tests.Json).GetTypeInfo();
            


        //    json.Read(t.GetMethod("DummyTest"));

        //    // Assert
        //    Assert.True(true);
        //}

        [Theory]
        [JsonDataSource]
        public void DummyTest(int a) {

            Assert.True(true);

        }

    }
}
