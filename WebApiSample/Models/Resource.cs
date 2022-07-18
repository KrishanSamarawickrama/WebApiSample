using System.Text.Json.Serialization;

namespace WebApiSample.Models;

public abstract class Resource : Link
{
    [JsonIgnore]
    public Link? Self { get; set; }
}