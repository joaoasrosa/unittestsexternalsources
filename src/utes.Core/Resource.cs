using System;
using System.Reflection;

namespace utes.Core
{
    /// <summary>
    /// Resource helper. Take from http://stackoverflow.com/questions/1150874/c-sharp-attribute-text-from-resource-file
    /// </summary>
    internal static class Resource
    {
        /// <summary>
        /// Metho to lookup resources.
        /// </summary>
        /// <typeparam name="T">The type to return.</typeparam>
        /// <param name="resourceType">The resource type.</param>
        /// <param name="resourceName">The resource name.</param>
        /// <returns>The resource content.</returns>
        internal static T GetResourceLookup<T>(Type resourceType, string resourceName)
        {
            if ((resourceType == null) || (resourceName == null)) return default(T);
            
            var property = resourceType.GetRuntimeProperty(resourceName);

            if (property == null)
            {
                return default(T);
            }

            return (T)property.GetValue(null, null);
        }
    }
}
