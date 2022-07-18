using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using WebApiSample.Models;

namespace WebApiSample.Filters;

public class JsonExceptionFilter : IExceptionFilter
{
    private readonly IHostEnvironment _env;

    public JsonExceptionFilter(IHostEnvironment env)
    {
        _env = env;
    }
    
    public void OnException(ExceptionContext context)
    {
        ApiError? output = null;
        if (_env.IsDevelopment())
            output = new ApiError()
            {
                Message = context.Exception.Message,
                Detail = context.Exception.StackTrace
            };
        else
            output = new ApiError()
            {
                Message = "Internal server error",
                Detail = context.Exception.Message
            };

        context.Result = new ObjectResult(output)
        {
            StatusCode = 500
        };
    }
}