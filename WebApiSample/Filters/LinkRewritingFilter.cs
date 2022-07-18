using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Routing;
using WebApiSample.Infrastructure;
using WebApiSample.Models;

namespace WebApiSample.Filters;

public class LinkRewritingFilter : IAsyncResultFilter
{
    private readonly IUrlHelperFactory _urlHelperFactory;

    public LinkRewritingFilter(IUrlHelperFactory urlHelperFactory)
    {
        _urlHelperFactory = urlHelperFactory;
    }

    public Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
    {
        var asObjectResults = context.Result as ObjectResult;
        bool shouldSkip = asObjectResults?.StatusCode >= 400
                          || asObjectResults?.Value == null
                          || asObjectResults?.Value is not Resource;

        if (shouldSkip) return next();

        var rewriter = new LinkRewriter(_urlHelperFactory.GetUrlHelper(context));
        RewriteAllLinks(asObjectResults?.Value, rewriter);

        return next();
    }

    private static void RewriteAllLinks(object? model, LinkRewriter rewriter)
    {
        if(model == null) return;

        var allProps = model.GetType()
            .GetTypeInfo()
            .GetProperties()
            .Where(p => p.CanRead)
            .ToList();

        var linkProperties = allProps.Where(p => p.PropertyType == typeof(Link)).ToList();
        
        linkProperties.ForEach(linkProp =>
        {
            var rewritten = rewriter.Rewrite(linkProp.GetValue(model) as Link);
            if(rewritten == null) return;
            linkProp.SetValue(model, rewritten);

            if (linkProp.Name != nameof(Resource.Self)) return;
            
            allProps.SingleOrDefault(p => p.Name == nameof(Resource.Href))
                ?.SetValue(model, rewritten.Href);
                
            allProps.SingleOrDefault(p => p.Name == nameof(Resource.Method))
                ?.SetValue(model, rewritten.Method);
                
            allProps.SingleOrDefault(p => p.Name == nameof(Resource.Relations))
                ?.SetValue(model, rewritten.Relations);
        });

        var arrayProperties = model.GetType().GetTypeInfo().GetProperties()
            .Where(p => p.CanRead && p.PropertyType.IsArray).ToList();
        
        RewriteLinksInArrays(arrayProperties, model, rewriter);

        var objectProperties = model.GetType().GetTypeInfo().GetProperties()
            .Except(linkProperties)
            .Except(arrayProperties);
        RewriteLinksInNestedObjects(objectProperties, model, rewriter);
    }
    
    private static void RewriteLinksInNestedObjects(
        IEnumerable<PropertyInfo> objectProperties,
        object model,
        LinkRewriter rewriter)
    {
        foreach (var objectProperty in objectProperties)
        {
            if (objectProperty.PropertyType == typeof(string))
            {
                continue;
            }

            var typeInfo = objectProperty.PropertyType.GetTypeInfo();
            if (typeInfo.IsClass)
            {
                RewriteAllLinks(objectProperty.GetValue(model), rewriter);
            }
        }
    }

    private static void RewriteLinksInArrays(
        IEnumerable<PropertyInfo> arrayProperties,
        object model,
        LinkRewriter rewriter)
    {

        foreach (var arrayProperty in arrayProperties)
        {
            var array = arrayProperty.GetValue(model) as Array ?? new Array[0];

            foreach (var element in array)
            {
                RewriteAllLinks(element, rewriter);
            }
        }
    }

}