namespace Ordering.Application.Models
{
    /// <summary>
    /// The email setting.
    /// </summary>
    public class EmailSettings
    {
        /// <summary>
        ///  Your Twilio SendGrid API key.
        /// </summary>
        public string ApiKey { get; set; } = string.Empty;

        /// <summary>
        /// Email sender address.
        /// </summary>
        public string FromAddress { get; set; } = string.Empty;

        /// <summary>
        /// A sender name.
        /// </summary>
        public string FromName { get; set; } = string.Empty;
    }
}
