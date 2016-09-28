using Xunit;

namespace utes.WebApp.Tests.Controllers
{
    /// <summary>
    /// Class to hold the tests for the Assembly controller
    /// </summary>
    public class AssemblyController
    {
        [Fact]
        public void Dummy()
        {
            // Arrange
            var assemblyController = new WebApp.Controllers.AssemblyController(null, null);
            
            // Act

            // Assert
            Assert.False(false);
        }
    }
}
