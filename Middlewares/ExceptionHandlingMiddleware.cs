using System.Net;
using Newtonsoft.Json;

namespace SheetIncision.Backend.Middlewares;

public class ExceptionHandlingMiddleware(RequestDelegate requestDelegate, ILogger<ExceptionHandlingMiddleware> logger)
{
    public RequestDelegate RequestDelegate = requestDelegate;

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await RequestDelegate(context);
        }
        catch (Exception ex)
        {
            await HandleException(context, ex);
        }
    }
    private Task HandleException(HttpContext context, Exception ex)
    {
        logger.LogError(ex.ToString());
        var errorMessageObject = new { Message = ex.Message, Code = "system_error" };

        var errorMessage = JsonConvert.SerializeObject(errorMessageObject);
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        return context.Response.WriteAsync(errorMessage);
    }
}