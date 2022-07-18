using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebApiSample.Filters;

public class HttpsOrCloseFilter : RequireHttpsAttribute
{
    protected override void HandleNonHttpsRequest(AuthorizationFilterContext filterContext)
    {
        filterContext.Result = new StatusCodeResult(400);
    }
}