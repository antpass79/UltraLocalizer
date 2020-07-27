using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Serilog;

namespace Globe.Identity.Middlewares
{
    public class JsonExceptionHandler
    {
        private readonly ILogger _logger;

        public JsonExceptionHandler(ILogger logger)
        {
            _logger = logger;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var ex = httpContext.Features.Get<IExceptionHandlerFeature>()?.Error;

            if (ex == null)
            {
                return;
            }

            var error = new ApiServerError
            {
                Message = ex.Message,
                StackTrace = ex.StackTrace,
                Details = ex.InnerException != null ? ex.InnerException.Message : string.Empty,
                StatusCode = (HttpStatusCode)httpContext.Response.StatusCode
            };

            _logger.Error(ex, "Unhandled exception inside the server");

            httpContext.Response.ContentType = "application/json";

            using (var writer = new StreamWriter(httpContext.Response.Body))
            {
                await JsonSerializer.SerializeAsync(writer.BaseStream, error);
                await writer.FlushAsync().ConfigureAwait(false);
            }
        }
    }

}
