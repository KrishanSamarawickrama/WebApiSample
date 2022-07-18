using System.ComponentModel;
using System.Text.Json.Serialization;

namespace WebApiSample.Models;

public class Link
{
    private const string GetMethod = "GET";
    
    [JsonPropertyOrder(-3)]
    public string? Href { get; set; }
    
    [JsonPropertyOrder(-2),JsonPropertyName("rel"),JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string[]? Relations { get; set; }
    
    [JsonPropertyOrder(-1), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [DefaultValue(GetMethod)]
    public string? Method { get; set; }

    [JsonIgnore]
    public string? RouteName { get; set; }
    
    [JsonIgnore]
    public object? RouteValue { get; set; }

    public static Link To(string routeName, object? routeValues = null) => new Link()
    {
        RouteName = routeName,
        RouteValue = routeValues,
        Method = GetMethod,
        Relations = null
    };
    
    public static Link ToCollection(string routeName, object? routeValues = null) => new Link()
    {
        RouteName = routeName,
        RouteValue = routeValues,
        Method = GetMethod,
        Relations = new []{"collection"}
    };
}