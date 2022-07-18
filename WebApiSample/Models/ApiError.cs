namespace WebApiSample.Models;

public class ApiError
{
    public string Message { get; set; } = default!;
    public string? Detail { get; set; }
}