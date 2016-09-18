using System.Reflection;
using System.Runtime.Loader;

namespace utes.WebApplicationAssemblyStorage
{
    /// <summary>
    /// Custom assembly load context to avoid the issue 5837.
    /// For more information check: https://github.com/dotnet/coreclr/issues/5837
    /// </summary>
    public class MyAssemblyLoadContext : AssemblyLoadContext
    {
        protected override Assembly Load(AssemblyName assemblyName)
        {
            return null;
        }
    }
}
