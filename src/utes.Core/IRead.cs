using System.Collections.Generic;

namespace utes.Core
{
    /// <summary>
    /// Interface to read from the external data source
    /// </summary>
    public interface IRead<T>
    {
        /// <summary>
        /// Method to read from the external data source
        /// </summary>
        /// <returns>The collection of objects of <typeparamref name="T"/>T</returns>
        IEnumerable<T> Read();
    }
}
