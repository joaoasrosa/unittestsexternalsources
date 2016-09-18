using Newtonsoft.Json;

namespace utes.WebApp.Models
{
    /// <summary>
    /// Model representing the information for the assembly upload.
    /// </summary>
    public class AssemblyUpload
    {
        /// <summary>
        /// Flag to indicate the success of the operation.
        /// </summary>
        [JsonProperty("sucess")]
        public bool Success { get; set; }

        /// <summary>
        /// Page to rediret.
        /// </summary>
        [JsonProperty("redirectTo")]
        public string RedirectTo { get; set; }

        /// <summary>
        /// The error heading.
        /// </summary>
        [JsonProperty("errorHeading")]
        public string ErrorHeading { get; set; }

        /// <summary>
        /// The error message.
        /// </summary>
        [JsonProperty("errorMessage")]
        public string ErrorMessage { get; set; }
    }
}
