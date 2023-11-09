using System.Net;
using System.Text;
using Application.Exceptions;
using Serilog;

namespace BazArtAPI.Middleware
{
    public class ErrorHandlingMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next.Invoke(context);
            }
            catch (NotFoundException notFoundException)
            {
                Log.Warning(notFoundException.Message);

                context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                await context.Response.WriteAsync(notFoundException.Message);
            }
            catch (BadRequestException badRequestException)
            {
                Log.Error(badRequestException.Message);

                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                await context.Response.WriteAsync(badRequestException.Message);
            }
            catch (ForbiddenAccessException forbiddenAccessException)
            {
                Log.Warning(forbiddenAccessException.Message);

                context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                await context.Response.WriteAsync(forbiddenAccessException.Message);
            }
            catch (ValidationException validationException)
            {
                var message = new StringBuilder();
                message.Append("Validation errors:\n");

                foreach (var (errorKey, errorValue) in validationException.Errors["errors"])
                {
                    foreach (var error in errorValue)
                    {
                        message.Append($"{errorKey} - {error} \n");
                    }
                }

                Log.Warning(message.ToString());

                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                await context.Response.WriteAsJsonAsync(validationException.Errors);
            }
            catch (Exception exception)
            {
                Log.Fatal($"| MESSAGE: {exception.Message} |\n| STACK TRACE: {exception.StackTrace} |");

                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                await context.Response.WriteAsync("Something went wrong");
            }
        }
    }
}
