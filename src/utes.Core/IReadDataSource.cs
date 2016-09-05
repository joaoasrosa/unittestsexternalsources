using System.Collections.Generic;
using System.Reflection;

namespace utes.Core
{
    /// <summary>
    /// Interface to read from the external data source.
    /// </summary>
    public interface IReadDataSource
    {
        /// <summary>
        /// Method to read from the external data source.
        /// </summary>
        /// <param name="methodInfo">The invoked method.</param>
        /// <returns>The collection of objects.</returns>
        IEnumerable<object[]> Read(MethodInfo methodInfo);
    }
}
