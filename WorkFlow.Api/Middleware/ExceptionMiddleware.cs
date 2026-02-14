using System.Net;
using System.Text.Json;
using WorkFlow.Application.Exceptions;

namespace WorkFlow.Api.Middleware
{
    public class ExceptionMiddleware(RequestDelegate _next)
    {
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (BusinessException ex)
            {
                await HandleExceptionAsync(context, HttpStatusCode.BadRequest, ex.Message);
            }
            catch (NotFoundException ex)
            {
                await HandleExceptionAsync(context, HttpStatusCode.BadRequest, ex.Message);
            }
            catch(Exception ex)
            {
                await HandleExceptionAsync(context, HttpStatusCode.InternalServerError, ex.Message);
                Console.WriteLine(ex.Message);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, HttpStatusCode statusCode, string message)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            var response = new { statusCode = context.Response.StatusCode, error = message, };

            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    } 
}
