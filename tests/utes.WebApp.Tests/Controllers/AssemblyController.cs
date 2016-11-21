using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Internal;
using Moq;
using utes.Domain;
using utes.Interfaces;
using Xunit;

namespace utes.WebApp.Tests.Controllers
{
    /// <summary>
    /// Class to hold the tests for the Assembly controller
    /// </summary>
    public class AssemblyController
    {
        /// <summary>
        /// Test method for the Assembly Controller constructor with null arguments.
        /// </summary>
        [Fact]
        public void ConstructorNullAssemblyStorageTest()
        {
            try
            {
                // Arrange
                // Act
                var assemblyController = new WebApp.Controllers.AssemblyController(null, null);
            }
            catch (ArgumentNullException argumentNullException)
            {
                // Assert
                Assert.NotNull(argumentNullException);
                Assert.Equal("Value cannot be null.\r\nParameter name: assemblyStorage", argumentNullException.Message);
            }
        }

        /// <summary>
        /// Test method for the Assembly Controller constructor with null logger argument.
        /// </summary>
        [Fact]
        public void ConstructorNullLoggerTest()
        {
            try
            {
                // Arrange
                var assemblyStorageMock = new Mock<IAssemblyStorage>();

                // Act
                var assemblyController = new WebApp.Controllers.AssemblyController(assemblyStorageMock.Object, null);
            }
            catch (ArgumentNullException argumentNullException)
            {
                // Assert
                Assert.NotNull(argumentNullException);
                Assert.Equal("Value cannot be null.\r\nParameter name: logger", argumentNullException.Message);
            }
        }

        /// <summary>
        /// Test method for the Assembly Controller constructor.
        /// </summary>
        [Fact]
        public void ConstructorTest()
        {
            // Arrange
            var assemblyStorageMock = new Mock<IAssemblyStorage>();
            var loggerMock = new Mock<ILogger<WebApp.Controllers.AssemblyController>>();

            // Act
            var assemblyController = new WebApp.Controllers.AssemblyController(assemblyStorageMock.Object,
                loggerMock.Object);

            // Assert
            Assert.True(true);
        }

        /// <summary>
        /// Test method for Index action in Assembly controller with an exception.
        /// </summary>
        [Fact]
        public void IndexExceptionTest()
        {
            Mock<ILogger<WebApp.Controllers.AssemblyController>> loggerMock = null;
            try
            {
                // Arrange
                var assemblyStorageMock = new Mock<IAssemblyStorage>();
                assemblyStorageMock.Setup(x => x.GetAssemblies()).Throws(new Exception("Error test!"));

                loggerMock = new Mock<ILogger<WebApp.Controllers.AssemblyController>>();
                loggerMock.Setup(x => x.Log(
                    It.Is<LogLevel>(y => y == LogLevel.Error),
                    It.Is<Microsoft.Extensions.Logging.EventId>(y => Equals(y, EventId.GenericException)),
                    It.IsAny<FormattedLogValues>(),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<object, Exception, string>>()));

                var assemblyController = new WebApp.Controllers.AssemblyController(assemblyStorageMock.Object,
                    loggerMock.Object);

                // Act
                assemblyController.Index();
            }
            catch (Exception exception)
            {
                // Assert
                Assert.NotNull(exception);
                Assert.IsType(typeof(Exception), exception);
                Assert.Equal("Error test!", exception.Message);
                Assert.NotNull(loggerMock);
                loggerMock.Verify(x => x.Log(
                    It.Is<LogLevel>(y => y == LogLevel.Error),
                    It.Is<Microsoft.Extensions.Logging.EventId>(y => Equals(y, EventId.GenericException)),
                    It.IsAny<FormattedLogValues>(),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<object, Exception, string>>()));
            }
        }

        /// <summary>
        /// Test method for Index action in Assembly controller.
        /// </summary>
        [Fact]
        public void IndexTest()
        {
            // Arrange
            var assemblyStorageMock = new Mock<IAssemblyStorage>();
            assemblyStorageMock.Setup(x => x.GetAssemblies()).Returns(new[]
            {
                new Assembly
                {
                    Path = @"C:\Temp\Dummy.dll",
                    Name = "Dummy.dll",
                    Version = "1.0.0.0"
                },
            });

            var loggerMock = new Mock<ILogger<WebApp.Controllers.AssemblyController>>();

            var assemblyController = new WebApp.Controllers.AssemblyController(assemblyStorageMock.Object,
                loggerMock.Object);

            // Act
            var result = assemblyController.Index() as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Model);
            Assert.IsType(typeof(Assembly[]), result.Model);
            Assert.NotEmpty((IEnumerable<Assembly>)result.Model);
            Assert.Equal(1, ((IEnumerable<Assembly>)result.Model).Count());

            var assembly = ((IEnumerable<Assembly>)result.Model).Single();
            Assert.Equal(@"C:\Temp\Dummy.dll", assembly.Path);
            Assert.Equal("Dummy.dll", assembly.Name);
            Assert.Equal("1.0.0.0", assembly.Version);

            Assert.Null(result.ContentType);
            Assert.Null(result.TempData);
            Assert.Null(result.ViewEngine);
            Assert.Null(result.ViewName);
            Assert.NotNull(result.ViewData);
            Assert.Empty(result.ViewData);
        }

        /// <summary>
        /// Test method for Upload action in Assembly controller.
        /// </summary>
        [Fact]
        public void UploadTest()
        {
            // Arrange
            var assemblyStorageMock = new Mock<IAssemblyStorage>();
            var loggerMock = new Mock<ILogger<WebApp.Controllers.AssemblyController>>();
            var assemblyController = new WebApp.Controllers.AssemblyController(assemblyStorageMock.Object,
                loggerMock.Object);

            // Act
            var result = assemblyController.Upload() as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.Null(result.Model);
        }

        /// <summary>
        /// Test method for UploadAssembly action in Assembly controller with an exception.
        /// </summary>
        [Fact]
        public void UploadAssemblyExceptionTest()
        {
            // Arrange
            var assemblyStorageMock = new Mock<IAssemblyStorage>();
            var loggerMock = new Mock<ILogger<WebApp.Controllers.AssemblyController>>();
            var assemblyController = new WebApp.Controllers.AssemblyController(assemblyStorageMock.Object,
                loggerMock.Object);


            var xpto = new Mock<ControllerContext>();
            var context = new Mock<HttpContext>();
            var request = new Mock<HttpRequest>();
            context
                .Setup(c => c.Request)
                .Returns(request.Object);

            xpto.Setup(x => x.HttpContext).Returns(context.Object);

            assemblyController.ControllerContext = xpto.Object;

            // Act
            var result = assemblyController.UploadAssemblyAsync().Result as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.Null(result.Model);
        }
    }
}
