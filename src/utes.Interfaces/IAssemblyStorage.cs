using System.Collections.Generic;
using utes.Domain;

namespace utes.Interfaces
{
    /// <summary>
    /// Interface to implement the operations over the assemblies storage.
    /// </summary>
    public interface IAssemblyStorage
    {
        /// <summary>
        /// Method to get the assemblies.
        /// </summary>
        /// <returns>The assemblies.</returns>
        IEnumerable<Assembly> GetAssemblies();

        /// <summary>
        /// Method to save an assembly.
        /// </summary>
        /// <param name="assembly">The assembly to save.</param>
        void SaveAssembly(Assembly assembly);

        /// <summary>
        /// Returns all the classes with methods implementing IMethodAttribute interface in the given assembly.
        /// </summary>
        /// <param name="assemblyName">The assembly name.</param>
        /// <returns>The classes with methods implementing IMethodAttribute interface.</returns>
        IEnumerable<Class> GetClassesInAssembly(string assemblyName);
    }
}
