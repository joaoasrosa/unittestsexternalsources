namespace utes.Domain
{
    /// <summary>
    /// Represent a method parameter.
    /// </summary>
    public class MethodParameter
    {
        /// <summary>
        /// The parameter name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The parameter type.
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// The method name.
        /// </summary>
        public string MethodName { get; set; }

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
