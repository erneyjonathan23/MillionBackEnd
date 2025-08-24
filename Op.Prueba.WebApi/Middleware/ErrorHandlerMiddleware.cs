using OP.Prueba.Application.Wrappers;
using System.Net;
using System.Text.Json;

namespace OP.Prueba.WebAPI.Middleware
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlerMiddleware> _logger;

        public ErrorHandlerMiddleware(RequestDelegate next, ILogger<ErrorHandlerMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception error)
            {
                var response = context.Response;
                response.ContentType = "application/json";
                var responseModel = new Response<string>() { Succeeded = false, Message = error?.Message!, HttpCode = HttpStatusCode.InternalServerError };
                switch (error)
                {
                    case Application.Exceptions.ApiException e:
                        // custom application error
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                        break;
                    case Application.Exceptions.ValidationException e:
                        // custom application error
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                        responseModel.Erros = e.Errors;
                        break;
                    case KeyNotFoundException e:
                        // not found error
                        response.StatusCode = (int)HttpStatusCode.NotFound;
                        break;
                    default:
                        // unhandled error
                        response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        break;
                }

                await SetContext(response.StatusCode, error);

                var result = JsonSerializer.Serialize(responseModel);
                await response.WriteAsync(result);
            }
        }

        private async Task SetContext(int statusCode, Exception ex)
        {
            _logger.LogError(
                "Error en ErrorHandlerMiddleware a las {datetime}, con código de estado {statusCode} y excepción {exception}",
                DateTime.Now, statusCode, ex);

            Console.WriteLine(ex?.Message);
        }
    }
}
