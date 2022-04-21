using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System.Net;

namespace Common.BuildApplication
{
    /// <summary>
    /// The problem detail builder service.
    /// </summary>
    public static class ProblemDetailBuilder
    {
        /// <summary>
        /// Add an exception handler and return problem details.
        /// </summary>
        /// <param name="services"> A service collection. </param>
        /// <returns> A service collection. </returns>
        public static IServiceCollection AddExceptionHandlerProblemDetail(this IServiceCollection services)
        {
            services.AddProblemDetails(options => {
                options.IncludeExceptionDetails = (ctx, exception) =>
                {
                    var environment = ctx.RequestServices.GetRequiredService<IHostEnvironment>();
                    var excludeDetailsFor = new HashSet<Type> {
                    typeof(KeyNotFoundException),
                    typeof(ArgumentException),
                    typeof(ArgumentNullException),
                    typeof(ArgumentOutOfRangeException),
                    typeof(DuplicateWaitObjectException),
                    typeof(HttpRequestException),
                };
                    return (environment.IsDevelopment() || environment.IsStaging())
                            && !excludeDetailsFor.TryGetValue(exception.GetType(), out Type? valueType);
                };

                options.ShouldLogUnhandledException = (_, exception, problemDetails) =>
                {
                    if (problemDetails?.Status.HasValue == true && problemDetails.Status.Value < 500)
                    {
                        // Logging a non-problem server exception as a warning.
                        Log.Warning(exception, exception.Message);
                        return false;
                    }

                    // Log the exception as unhandled.
                    return true;
                };

                options.ValidationProblemStatusCode = StatusCodes.Status400BadRequest;

                static ProblemDetails Map<TException>(TException exception, HttpStatusCode statusCode) where TException : Exception =>
                    new StatusCodeProblemDetails((int)statusCode) { Detail = exception.Message };

                options.Map<KeyNotFoundException>(exception => Map(exception, HttpStatusCode.NotFound));
                options.Map<ArgumentException>(exception => Map(exception, HttpStatusCode.BadRequest));
                options.Map<InvalidOperationException>(exception => Map(exception, HttpStatusCode.BadRequest));
                options.Map<HttpRequestException>(exception => Map(exception, exception.StatusCode ?? 0));
                options.Map<NotImplementedException>(exception => Map(exception, HttpStatusCode.NotImplemented));
                options.Map<Exception>(exception => Map(exception, HttpStatusCode.InternalServerError));
            });

            return services;
        }
    }
}
