using MediatR;
using Microsoft.Extensions.Logging;

namespace Ordering.Application.Behaviours
{
    /// <summary>
    /// The unhandled exception behaviour.
    /// </summary>
    /// <typeparam name="TRequest"> A request type. </typeparam>
    /// <typeparam name="TResponse"> A response type. </typeparam>
    public class UnhandledExceptionBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly ILogger<TRequest> _logger;

        /// <summary>
        /// Initialization.
        /// </summary>
        /// <param name="logger"> A logging service. </param>
        public UnhandledExceptionBehaviour(ILogger<TRequest> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// The unhandled exception handler.
        /// </summary>
        /// <param name="request"> A request. </param>
        /// <param name="cancellationToken"> Propagates notification that operations should be canceled. </param>
        /// <param name="next"> A next handler request. </param>
        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            try
            {
                return await next();
            }
            catch (KeyNotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                var requestName = typeof(TRequest).Name;
                _logger.LogError(ex, "Application Request: Unhandled Exception for Request {Name} {@Request}", requestName, request);
                throw;
            }
        }
    }
}
