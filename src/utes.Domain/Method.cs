namespace utes.Domain
{
    /// <summary>
    /// Representd a class method that implements the IMethodAttribute interface.
    /// </summary>
    public class Method
    {
        /// <summary>
        /// The method name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The class name.
        /// </summary>
        public string ClassName { get; set; }

        /// <summary>
        /// The assembly name.
        /// </summary>
        public string AssemblyName { get; set; }
    }
}
