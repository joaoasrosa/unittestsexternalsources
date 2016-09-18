namespace utes.WebApp
{
    internal static class EventId
    {
        /// <summary>
        /// Generic exception event id.
        /// </summary>
        internal static Microsoft.Extensions.Logging.EventId GenericException
            => new Microsoft.Extensions.Logging.EventId(1000, "Generic error");

        /// <summary>
        /// Successfully assembly upload event id.
        /// </summary>
        internal static Microsoft.Extensions.Logging.EventId SucessfullyAssemblyUpload
            => new Microsoft.Extensions.Logging.EventId(1001, "Sucessfully assembly upload");

        /// <summary>
        /// Exception assembly upload event id.
        /// </summary>
        internal static Microsoft.Extensions.Logging.EventId ExceptionAssemblyUpload
            => new Microsoft.Extensions.Logging.EventId(1002, "Exception uploading assembly");
    }
}
