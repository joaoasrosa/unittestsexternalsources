using System.Collections.Generic;
using System.Threading.Tasks;
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
    }
}
