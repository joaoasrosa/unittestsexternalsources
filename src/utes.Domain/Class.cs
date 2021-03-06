﻿namespace utes.Domain
{
    /// <summary>
    /// Represents a Class in the domain.
    /// </summary>
    public class Class
    {
        /// <summary>
        /// The name of the class.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The full name of the class.
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// The assembly name.
        /// </summary>
        public string AssemblyName { get; set; }
    }
}
