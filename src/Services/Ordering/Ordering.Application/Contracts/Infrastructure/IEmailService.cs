using Ordering.Application.Models;

namespace Ordering.Application.Contracts.Infrastructure
{
    /// <summary>
    /// The email service.
    /// </summary>
    public interface IEmailService
    {
        /// <summary>
        /// Send an email.
        /// </summary>
        /// <param name="email"> An email. </param>
        /// <returns> The result of sending. </returns>
        Task<bool> SendEmail(Email email);
    }
}
