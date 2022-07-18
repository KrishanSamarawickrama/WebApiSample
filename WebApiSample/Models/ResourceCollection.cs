namespace WebApiSample.Models;

public class ResourceCollection<T> : Resource
{
    public T[]? Value { get; set; }
}