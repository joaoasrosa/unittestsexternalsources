using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Loader;
using Microsoft.AspNetCore.Hosting;
using utes.Domain;
using utes.Interfaces;

namespace utes.WebApplicationAssemblyStorage
{
    /// <summary>
    /// Class to handle the assembly storage within the web application.
    /// </summary>
    public class WebApplicationAssemblyStorage : IAssemblyStorage
    {
        private readonly string _assembliesPath;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="appEnvironment">The hosting environment interface.</param>
        public WebApplicationAssemblyStorage(IHostingEnvironment appEnvironment)
        {
            // If the path doesn't exist, create it
            this._assembliesPath = Path.Combine(appEnvironment.ContentRootPath, "assemblies");
            if (!Directory.Exists(this._assembliesPath))
            {
                Directory.CreateDirectory(this._assembliesPath);
            }
        }

        /// <summary>
        /// Method to get the assemblies.
        /// </summary>
        /// <returns>The assemblies.</returns>
        public IEnumerable<Assembly> GetAssemblies()
        {
            // Get all the assemblies
            var assembliesPath = Directory.EnumerateFiles(this._assembliesPath, "*.dll");

            return (from assemblyPath in assembliesPath
                    let assembly = AssemblyLoadContext.GetAssemblyName(assemblyPath)
                    select new Assembly
                    {
                        Path = assemblyPath,
                        Name = assembly.Name,
                        Version = assembly.Version.ToString()
                    }).ToArray();
        }

        /// <summary>
        /// Method to save an assembly.
        /// </summary>
        /// <param name="assembly">The assembly to save.</param>
        public void SaveAssembly(Assembly assembly)
        {
            var assemblyPath = Path.Combine(this._assembliesPath, assembly.Name);
            File.WriteAllBytes(assemblyPath, assembly.ContentBytes);
        }
    }
}
