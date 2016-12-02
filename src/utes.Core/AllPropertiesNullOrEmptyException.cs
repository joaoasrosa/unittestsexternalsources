using System;

namespace utes.Core
{
    public class AllPropertiesNullOrEmptyException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the utes.Core.AllPropertiesNullOrEmptyException class.
        /// </summary>
        public AllPropertiesNullOrEmptyException() : base(GlobalResource.ResourceManager.GetString("AllPropertiesNullOrEmptyExceptionMessage"))
        {
        }
    }
}
