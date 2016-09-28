using System;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Moq;
using utes.Core;
using utes.Domain;
using Xunit;

namespace utes.WebApplicationAssemblyStorage.Tests
{
    public class WebApplicationAssemblyStorage : IDisposable
    {
        private const string NonExistentTestDllDirectory = @"C:\Temp\NonUnitTestExternalSources";
        private const string TestDllDirectory = @"C:\Temp\UnitTestExternalSources";
        private const string DllDirectory = @"C:\Temp\UnitTestExternalSources\assemblies";
        private const string AssemblyNameWithExtension = "utes.WebApplicationAssemblyStorage.Tests.dll";
        private const string AssemblyName = "utes.WebApplicationAssemblyStorage.Tests";

        public WebApplicationAssemblyStorage()
        {
            // Clean up from some erroneous test run.
            if (Directory.Exists(DllDirectory))
            {
                Directory.Delete(DllDirectory, true);
            }

            if (!Directory.Exists(DllDirectory))
            {
                Directory.CreateDirectory(DllDirectory);
            }

            // Deploy known DLL's.
            var knownDlls = Directory.EnumerateFiles(AppContext.BaseDirectory, AssemblyNameWithExtension);
            foreach (var knownDll in knownDlls)
            {
                File.Copy(knownDll, Path.Combine(DllDirectory, Path.GetFileName(knownDll)));
            }
        }

        public void Dispose()
        {
            if (Directory.Exists(DllDirectory))
            {
                Directory.Delete(DllDirectory, true);
            }

            if (Directory.Exists(NonExistentTestDllDirectory))
            {
                Directory.Delete(NonExistentTestDllDirectory, true);
            }
        }

        /// <summary>
        /// Test method for WebApplicationAssemblyStorage with appEnvironmment null.
        /// </summary>
        [Fact]
        public void ConstructorNullAppEnvironment()
        {
            try
            {
                // Arrange
                // Act
                var webApplicationAssemblyStorage =
                    new utes.WebApplicationAssemblyStorage.WebApplicationAssemblyStorage(null, null);
            }
            catch (ArgumentNullException argumentNullException)
            {
                // Assert
                Assert.NotNull(argumentNullException);
                Assert.Equal("Value cannot be null.\r\nParameter name: appEnvironment", argumentNullException.Message);
            }
        }

        /// <summary>
        /// Test method for WebApplicationAssemblyStorage with methodAttributes null.
        /// </summary>
        [Fact]
        public void ConstructorNullMethodAttributes()
        {
            try
            {
                // Arrange
                var hostingEnvironmentMock = new Mock<IHostingEnvironment>();

                // Act
                var webApplicationAssemblyStorage =
                    new utes.WebApplicationAssemblyStorage.WebApplicationAssemblyStorage(hostingEnvironmentMock.Object,
                        null);
            }
            catch (ArgumentNullException argumentNullException)
            {
                // Assert
                Assert.NotNull(argumentNullException);
                Assert.Equal("Value cannot be null.\r\nParameter name: methodAttributes", argumentNullException.Message);
            }
        }

        /// <summary>
        /// Test method for WebApplicationAssemblyStorage .
        /// </summary>
        [Fact]
        public void Constructor()
        {
            try
            {
                // Arrange
                var hostingEnvironmentMock = new Mock<IHostingEnvironment>();
                hostingEnvironmentMock.Setup(x => x.ContentRootPath).Returns(NonExistentTestDllDirectory);

                var methodAttributeMock = new Mock<IMethodAttribute>();

                // Act
                var webApplicationAssemblyStorage = new utes.WebApplicationAssemblyStorage.WebApplicationAssemblyStorage
                (
                    hostingEnvironmentMock.Object, new[] { methodAttributeMock.Object });
            }
            catch (ArgumentNullException argumentNullException)
            {
                // Assert
                Assert.NotNull(argumentNullException);
                Assert.Equal("Value cannot be null.\r\nParameter name: methodAttributes", argumentNullException.Message);
            }
        }

        /// <summary>
        /// Test method for GetAssemblies method.
        /// </summary>
        [Fact]
        public void GetAssembliesTest()
        {
            // Arrange
            var hostingEnvironmentMock = new Mock<IHostingEnvironment>();
            hostingEnvironmentMock.Setup(x => x.ContentRootPath).Returns(TestDllDirectory);

            var methodAttributeMock = new Mock<IMethodAttribute>();

            var webApplicationAssemblyStorage = new utes.WebApplicationAssemblyStorage.WebApplicationAssemblyStorage(
                hostingEnvironmentMock.Object, new[] { methodAttributeMock.Object });

            // Act
            var assemblies = webApplicationAssemblyStorage.GetAssemblies();

            // Assert
            Assert.NotNull(assemblies);

            var assembliesArray = assemblies.ToArray();
            Assert.NotEmpty(assembliesArray);
            Assert.Equal(1, assembliesArray.Length);

            var assembly = assembliesArray[0];
            Assert.Null(assembly.ContentBytes);
            Assert.NotNull(assembly.Version);
            Assert.Equal(AssemblyName, assembly.Name);
            Assert.Equal(Path.Combine(DllDirectory, AssemblyNameWithExtension), assembly.Path);
        }

        /// <summary>
        /// Test method for SaveAssembly method with wrong assembly extension.
        /// </summary>
        [Fact]
        public void SaveAssemblyWrongException()
        {
            try
            {
                // Arrange
                var hostingEnvironmentMock = new Mock<IHostingEnvironment>();
                hostingEnvironmentMock.Setup(x => x.ContentRootPath).Returns(TestDllDirectory);

                var methodAttributeMock = new Mock<IMethodAttribute>();

                var webApplicationAssemblyStorage = new utes.WebApplicationAssemblyStorage.WebApplicationAssemblyStorage(
                    hostingEnvironmentMock.Object, new[] { methodAttributeMock.Object });

                var assembly = new Assembly
                {
                    Name = "HelloWorld.txt"
                };

                // Act
                webApplicationAssemblyStorage.SaveAssembly(assembly);
            }
            catch (BadImageFormatException badImageFormatException)
            {
                // Assert
                Assert.Equal("Unexpected file extension.", badImageFormatException.Message);
            }
        }


    }
}
