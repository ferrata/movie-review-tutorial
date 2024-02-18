using System.Net;
using GabrielesProject.MovieReviewSystem.Domain.Exceptions;

namespace GabrielesProject.MovieReviewSystem.WebApi.Middlewares;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (MovieNotFoundException ex)
        {
            context.Response.StatusCode = (int) HttpStatusCode.NotFound;
            await context.Response.WriteAsJsonAsync(new {Message = ex.Message});
        }
        catch (InvalidInputException ex)
        {
            context.Response.StatusCode = (int) HttpStatusCode.BadRequest;
            await context.Response.WriteAsJsonAsync(new {Message = ex.Message});
        }
        catch (Exception ex)
        {
            context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;
            await context.Response.WriteAsJsonAsync(new {Message = ex.Message});
        }
    }
}