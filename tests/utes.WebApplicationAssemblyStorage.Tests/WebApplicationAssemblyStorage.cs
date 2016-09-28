using System;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Moq;
using utes.Core;
using utes.Domain;
using utes.Xunit;
using Xunit;

namespace utes.WebApplicationAssemblyStorage.Tests
{
    public class WebApplicationAssemblyStorage : IDisposable
    {
        private const string NonExistentTestDllDirectory = @"C:\Temp\NonUnitTestExternalSources";
        private const string TestDllDirectory = @"C:\Temp\UnitTestExternalSources";
        private const string DllDirectory = @"C:\Temp\UnitTestExternalSources\assemblies";
        private const string DummyDllDirectory = @"C:\Temp\UnitTestExternalSources\dummyassemblies";
        private const string AssemblyNameWithExtension = "utes.WebApplicationAssemblyStorage.Tests.dll";
        private const string AssemblyName = "utes.WebApplicationAssemblyStorage.Tests";

        public WebApplicationAssemblyStorage()
        {
            // Clean up from some erroneous test run.
            try
            {
                if (Directory.Exists(DllDirectory))
                {
                    Directory.Delete(DllDirectory, true);
                }
            }
            catch
            {
                // ignored
            }
            
            // Clean up from some erroneous test run.
            try
            {
                if (Directory.Exists(DummyDllDirectory))
                {
                    Directory.Delete(DummyDllDirectory, true);
                }
            }
            catch
            {
                // ignored
            }

            if (!Directory.Exists(DllDirectory))
            {
                Directory.CreateDirectory(DllDirectory);
            }

            if (!Directory.Exists(DummyDllDirectory))
            {
                Directory.CreateDirectory(DummyDllDirectory);
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
            try
            {
                if (Directory.Exists(DllDirectory))
                {
                    Directory.Delete(DllDirectory, true);
                }

                if (Directory.Exists(DummyDllDirectory))
                {
                    Directory.Delete(DummyDllDirectory, true);
                }

                if (Directory.Exists(NonExistentTestDllDirectory))
                {
                    Directory.Delete(NonExistentTestDllDirectory, true);
                }
            }
            catch
            {
                // ignored
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
        public void SaveAssemblyWrongAssemblyExtensionException()
        {
            try
            {
                // Arrange
                var hostingEnvironmentMock = new Mock<IHostingEnvironment>();
                hostingEnvironmentMock.Setup(x => x.ContentRootPath).Returns(DummyDllDirectory);

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

        /// <summary>
        /// Test method for SaveAssembly method with wrong assembly file.
        /// </summary>
        [Fact]
        public void SaveAssemblyWrongAssemblyFileException()
        {
            try
            {
                // Arrange
                var hostingEnvironmentMock = new Mock<IHostingEnvironment>();
                hostingEnvironmentMock.Setup(x => x.ContentRootPath).Returns(DummyDllDirectory);

                var methodAttributeMock = new Mock<IMethodAttribute>();

                var webApplicationAssemblyStorage = new utes.WebApplicationAssemblyStorage.WebApplicationAssemblyStorage(
                    hostingEnvironmentMock.Object, new[] { methodAttributeMock.Object });

                var assembly = new Assembly
                {
                    Name = "HelloWorld.dll",
                    ContentBytes = new[] { byte.Parse("0"), byte.Parse("1") }
                };

                // Act
                webApplicationAssemblyStorage.SaveAssembly(assembly);
            }
            catch (BadImageFormatException badImageFormatException)
            {
                // Assert
                Assert.Equal(" is not a valid Win32 application. (Exception from HRESULT: 0x800700C1)", badImageFormatException.Message);
            }
        }

        /// <summary>
        /// Test method for SaveAssembly method with assembly file not implementing the interface.
        /// </summary>
        [Fact]
        public void SaveAssemblyAssemblyDoesNotImplementInterfaceException()
        {
            try
            {
                // Arrange
                var hostingEnvironmentMock = new Mock<IHostingEnvironment>();
                hostingEnvironmentMock.Setup(x => x.ContentRootPath).Returns(DummyDllDirectory);

                var methodAttributeMock = new Mock<IMethodAttribute>();

                var webApplicationAssemblyStorage = new utes.WebApplicationAssemblyStorage.WebApplicationAssemblyStorage(
                    hostingEnvironmentMock.Object, new[] { methodAttributeMock.Object });

                var dllPath = Directory.EnumerateFiles(AppContext.BaseDirectory, "*.dll").First();
                var assembly = new Assembly
                {
                    Name = "FooBar.dll",
                    ContentBytes = File.ReadAllBytes(dllPath)
                };

                // Act
                webApplicationAssemblyStorage.SaveAssembly(assembly);
            }
            catch (DataSourceAttributeNotFoundException dataSourceAttributeNotFoundException)
            {
                // Assert
                Assert.Equal("The uploaded assembly do not implement IMethodAttribute interface.", dataSourceAttributeNotFoundException.Message);
            }
        }

        /// <summary>
        /// Test method for SaveAssembly method.
        /// </summary>
        [Fact]
        public void SaveAssembly()
        {
            // Arrange
            var hostingEnvironmentMock = new Mock<IHostingEnvironment>();
            hostingEnvironmentMock.Setup(x => x.ContentRootPath).Returns(DummyDllDirectory);

            var webApplicationAssemblyStorage = new utes.WebApplicationAssemblyStorage.WebApplicationAssemblyStorage(
                hostingEnvironmentMock.Object, new[] { new JsonDataSourceAttribute() });

            var dllPath = Directory.EnumerateFiles(AppContext.BaseDirectory, "utes.Core.Tests.dll").First();
            var assembly = new Assembly
            {
                Name = "utes.Core.Tests.dll",
                ContentBytes = File.ReadAllBytes(dllPath)
            };

            // Act
            webApplicationAssemblyStorage.SaveAssembly(assembly);

            // Assert 
            // This case no exceptions.
            Assert.True(true);
        }
    }
}
