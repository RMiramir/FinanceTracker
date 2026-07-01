using FinanceTracker.Exceptions;

namespace FinanceTracker.Middlewares;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;
    
    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _logger = logger;
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            _logger.LogInformation("Начата обработка запроса {Method} {Path}", context.Request.Method,
                context.Request.Path);
            await _next(context);
        }
        catch (NotFoundException e)
        {
            _logger.LogError(e, "Объект не найден при обработке {Method} {Path} {Body}", context.Request.Method,
                context.Request.Path, context.Request.Body);
            await WriteErrorResponseAsync(context, StatusCodes.Status404NotFound, "Not Found", e.Message);
        }
        catch (ConflictException e)
        {
            _logger.LogError(e,
                "Произошла ошибка валидации или изменяемый или создаваемый объект уже существует при обработке запроса {Method} {Path}",
                context.Request.Method, context.Request.Path);

            await WriteErrorResponseAsync(context, StatusCodes.Status409Conflict, "Conflict", e.Message);
        }
        catch (BadRequestException e)
        {
            _logger.LogError(e, "Указаны неверные данные при обработке запроса {Method} {Path}", context.Request.Method,
                context.Request.Path);
            await WriteErrorResponseAsync(context, StatusCodes.Status400BadRequest, "Bad Request", e.Message);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Произошла непредвиденная внутренняя ошибка сервера при обработке запроса {Method} {Path}", 
                context.Request.Method, context.Request.Path);
            await WriteErrorResponseAsync(context, StatusCodes.Status500InternalServerError, 
                "Internal Server Error",
                "На сервере произошла непредвиденная ошибка. Мы уже работаем над её исправлением.");
        }
    }

    private async Task WriteErrorResponseAsync(HttpContext context, int statusCode, string errorMessage, string exceptionMessage)
    {
        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/json";

        var response = new
        {
            status = statusCode,
            error = errorMessage,
            message = exceptionMessage
        };

        await context.Response.WriteAsJsonAsync(response);
    }

}