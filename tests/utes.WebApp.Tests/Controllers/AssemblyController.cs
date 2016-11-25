using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Internal;
using Moq;
using NLog;
using utes.Domain;
using utes.Interfaces;
using utes.WebApp.Models;
using Xunit;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;

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
        /// Test method for UploadAssembly action in Assembly controller with no file.
        /// </summary>
        [Fact]
        public void UploadAssemblyNoFileTest()
        {
            // Arrange
            var assemblyStorageMock = new Mock<IAssemblyStorage>();
            var loggerMock = new Mock<ILogger<WebApp.Controllers.AssemblyController>>();
            var assemblyController = new WebApp.Controllers.AssemblyController(assemblyStorageMock.Object,
                loggerMock.Object);

            var formFileCollection = new Mock<IFormFileCollection>();
            formFileCollection.Setup(x => x.Count).Returns(0);

            var formCollection = new Mock<IFormCollection>();
            formCollection.Setup(x => x.Files).Returns(formFileCollection.Object);

            var httpRequest = new Mock<HttpRequest>();
            httpRequest.Setup(x => x.Form).Returns(formCollection.Object);

            var httpContext = new Mock<HttpContext>();
            httpContext.Setup(x => x.Request).Returns(httpRequest.Object);

            var controllerContext = new Mock<ControllerContext>();
            controllerContext.Object.HttpContext = httpContext.Object;
            assemblyController.ControllerContext = controllerContext.Object;

            // Act
            var result = assemblyController.UploadAssemblyAsync().Result as JsonResult;

            // Assert
            Assert.NotNull(result);
            Assert.NotNull((AssemblyUpload)result.Value);
            Assert.Equal(false, ((AssemblyUpload)result.Value).Success);
            Assert.Equal("No assembly uploaded.", ((AssemblyUpload)result.Value).ErrorHeading);
            Assert.Equal("Please upload a valid assembly file.", ((AssemblyUpload)result.Value).ErrorMessage);
        }

        /// <summary>
        /// Test method for UploadAssembly action in Assembly controller with an DataSourceAttributeNotFoundException.
        /// </summary>
        //TODO: Eat your own dog food!
        [Fact]
        public void UploadAssemblyDataSourceAttributeNotFoundExceptionTest()
        {
            // Arrange
            var assemblyStorageMock = new Mock<IAssemblyStorage>();
            assemblyStorageMock.Setup(x => x.SaveAssembly(It.IsAny<Assembly>())).Throws<DataSourceAttributeNotFoundException>();

            var loggerMock = new Mock<ILogger<WebApp.Controllers.AssemblyController>>();
            var assemblyController = new WebApp.Controllers.AssemblyController(assemblyStorageMock.Object,
                loggerMock.Object);

            var formFile = new Mock<IFormFile>();
            formFile.Setup(x => x.FileName).Returns("Test");

            var formFileCollection = new Mock<IFormFileCollection>();
            formFileCollection.Setup(x => x.GetFile("assemblyFile")).Returns(formFile.Object);
            formFileCollection.Setup(x => x.Count).Returns(1);

            var formCollection = new Mock<IFormCollection>();
            formCollection.Setup(x => x.Files).Returns(formFileCollection.Object);

            var httpRequest = new Mock<HttpRequest>();
            httpRequest.Setup(x => x.Form).Returns(formCollection.Object);

            var httpContext = new Mock<HttpContext>();
            httpContext.Setup(x => x.Request).Returns(httpRequest.Object);

            var controllerContext = new Mock<ControllerContext>();
            controllerContext.Object.HttpContext = httpContext.Object;
            assemblyController.ControllerContext = controllerContext.Object;

            // Act
            var result = assemblyController.UploadAssemblyAsync().Result as JsonResult;

            // Assert
            Assert.NotNull(result);
            Assert.NotNull((AssemblyUpload)result.Value);
            Assert.Equal(false, ((AssemblyUpload)result.Value).Success);
            Assert.Equal("The uploaded file is not valid.", ((AssemblyUpload)result.Value).ErrorHeading);
            Assert.Equal("Please upload a valid assembly file.", ((AssemblyUpload)result.Value).ErrorMessage);
        }

        /// <summary>
        /// Test method for UploadAssembly action in Assembly controller with an DataSourceAttributeNotFoundException.
        /// </summary>
        //TODO: Eat your own dog food!
        [Fact]
        public void UploadAssemblyBadImageFormatExceptionTest()
        {
            // Arrange
            var assemblyStorageMock = new Mock<IAssemblyStorage>();
            assemblyStorageMock.Setup(x => x.SaveAssembly(It.IsAny<Assembly>())).Throws<BadImageFormatException>();

            var loggerMock = new Mock<ILogger<WebApp.Controllers.AssemblyController>>();
            var assemblyController = new WebApp.Controllers.AssemblyController(assemblyStorageMock.Object,
                loggerMock.Object);

            var formFile = new Mock<IFormFile>();
            formFile.Setup(x => x.FileName).Returns("Test");

            var formFileCollection = new Mock<IFormFileCollection>();
            formFileCollection.Setup(x => x.GetFile("assemblyFile")).Returns(formFile.Object);
            formFileCollection.Setup(x => x.Count).Returns(1);

            var formCollection = new Mock<IFormCollection>();
            formCollection.Setup(x => x.Files).Returns(formFileCollection.Object);

            var httpRequest = new Mock<HttpRequest>();
            httpRequest.Setup(x => x.Form).Returns(formCollection.Object);

            var httpContext = new Mock<HttpContext>();
            httpContext.Setup(x => x.Request).Returns(httpRequest.Object);

            var controllerContext = new Mock<ControllerContext>();
            controllerContext.Object.HttpContext = httpContext.Object;
            assemblyController.ControllerContext = controllerContext.Object;

            // Act
            var result = assemblyController.UploadAssemblyAsync().Result as JsonResult;

            // Assert
            Assert.NotNull(result);
            Assert.NotNull((AssemblyUpload)result.Value);
            Assert.Equal(false, ((AssemblyUpload)result.Value).Success);
            Assert.Equal("The uploaded file is not valid.", ((AssemblyUpload)result.Value).ErrorHeading);
            Assert.Equal("Please upload a valid assembly file.", ((AssemblyUpload)result.Value).ErrorMessage);
        }

        /// <summary>
        /// Test method for UploadAssembly action in Assembly controller.
        /// </summary>
        [Fact]
        public void UploadAssemblyTest()
        {
            // Arrange
            var assemblyStorageMock = new Mock<IAssemblyStorage>();
            var loggerMock = new Mock<ILogger<WebApp.Controllers.AssemblyController>>();
            var assemblyController = new WebApp.Controllers.AssemblyController(assemblyStorageMock.Object,
                loggerMock.Object);

            var formFile = new Mock<IFormFile>();
            formFile.Setup(x => x.FileName).Returns("Test");

            var formFileCollection = new Mock<IFormFileCollection>();
            formFileCollection.Setup(x => x.GetFile("assemblyFile")).Returns(formFile.Object);
            formFileCollection.Setup(x => x.Count).Returns(1);

            var formCollection = new Mock<IFormCollection>();
            formCollection.Setup(x => x.Files).Returns(formFileCollection.Object);

            var httpRequest = new Mock<HttpRequest>();
            httpRequest.Setup(x => x.Form).Returns(formCollection.Object);

            var httpContext = new Mock<HttpContext>();
            httpContext.Setup(x => x.Request).Returns(httpRequest.Object);

            var controllerContext = new Mock<ControllerContext>();
            controllerContext.Object.HttpContext = httpContext.Object;
            assemblyController.ControllerContext = controllerContext.Object;

            // Act
            var result = assemblyController.UploadAssemblyAsync().Result as JsonResult;

            // Assert
            Assert.NotNull(result);
            Assert.NotNull((AssemblyUpload)result.Value);
            Assert.Equal(true, ((AssemblyUpload)result.Value).Success);
            Assert.Equal("/Assembly", ((AssemblyUpload)result.Value).RedirectTo);
        }
    }
}
