namespace Ordering.Application.Models
{
    /// <summary>
    /// An email entity.
    /// </summary>
    public class Email
    {
        /// <summary>
        /// An email address of the recipient.
        /// </summary>
        public string To { get; set; } = string.Empty;

        /// <summary>
        /// A subject.
        /// </summary>
        public string Subject { get; set; } = string.Empty;

        /// <summary>
        /// E body email.
        /// </summary>
        public string Body { get; set; } = string.Empty;
    }
}
