using System;

namespace utes.Core
{
    public class FileNotExistsException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the utes.Core.FileNotExistsException class.
        /// </summary>
        public FileNotExistsException() : base(GlobalResource.ResourceManager.GetString("FileNotExistsExceptionMessage"))
        {
        }
    }
}
