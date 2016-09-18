namespace utes.Domain
{
    /// <summary>
    /// Represents a Assembly in the domain.
    /// </summary>
    public class Assembly
    {
        /// <summary>
        /// The name of the assembly.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The path for the assembly.
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// The version of the assembly.
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// The byte content of th assembly.
        /// </summary>
        public byte[] ContentBytes { get; set; }
    }
}
