using System;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Moq;
using Xunit;

namespace utes.WebApplicationAssemblyStorage.Tests
{
    public class WebApplicationAssemblyStorage : IDisposable
    {
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

            var webApplicationAssemblyStorage = new utes.WebApplicationAssemblyStorage.WebApplicationAssemblyStorage(hostingEnvironmentMock.Object, null);
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
        /// Test method for SaveAssembly method.
        /// </summary>
        [Fact]
        public void SaveAssembly()
        {
            // TODO
        }
    }
}
