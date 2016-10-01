using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace utes.WebApp.Tests.Controllers
{
    /// <summary>
    /// Class to hold the tests for the Home controller
    /// </summary>
    public class HomeController
    {
        /// <summary>
        /// Test method for Index action in Home controller.
        /// </summary>
        [Fact]
        public void IndexTest()
        {
            // Arrange
            var controller = new WebApp.Controllers. HomeController();
            
            // Act
            var result = controller.Index() as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.Null(result.Model);
            Assert.Null(result.ContentType);
            Assert.Null(result.TempData);
            Assert.Null(result.ViewEngine);
            Assert.Null(result.ViewName);
            Assert.NotNull(result.ViewData);
            Assert.Empty(result.ViewData);
        }

        /// <summary>
        /// Test method for About action in Home controller.
        /// </summary>
        [Fact]
        public void AboutTest()
        {
            // Arrange
            var controller = new WebApp.Controllers.HomeController();

            // Act
            var result = controller.About() as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.Null(result.Model);
            Assert.Null(result.ContentType);
            Assert.Null(result.TempData);
            Assert.Null(result.ViewEngine);
            Assert.Null(result.ViewName);
            Assert.NotNull(result.ViewData);
            Assert.NotEmpty(result.ViewData);
            Assert.Equal(1, result.ViewData.Count);
            Assert.Equal("Your application description page.", result.ViewData["Message"]);
        }

        /// <summary>
        /// Test method for Contact action in Home controller.
        /// </summary>
        [Fact]
        public void ContactTest()
        {
            // Arrange
            var controller = new WebApp.Controllers.HomeController();

            // Act
            var result = controller.Contact() as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.Null(result.Model);
            Assert.Null(result.ContentType);
            Assert.Null(result.TempData);
            Assert.Null(result.ViewEngine);
            Assert.Null(result.ViewName);
            Assert.NotNull(result.ViewData);
            Assert.NotEmpty(result.ViewData);
            Assert.Equal(1, result.ViewData.Count);
            Assert.Equal("Your contact page.", result.ViewData["Message"]);
        }

        /// <summary>
        /// Test method for Error action in Home controller.
        /// </summary>
        [Fact]
        public void ErrorTest()
        {
            // Arrange
            var controller = new WebApp.Controllers.HomeController();

            // Act
            var result = controller.Error() as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.Null(result.Model);
            Assert.Null(result.ContentType);
            Assert.Null(result.TempData);
            Assert.Null(result.ViewEngine);
            Assert.Null(result.ViewName);
            Assert.NotNull(result.ViewData);
            Assert.Empty(result.ViewData);
        }
    }
}
