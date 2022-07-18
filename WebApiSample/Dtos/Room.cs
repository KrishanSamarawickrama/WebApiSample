using WebApiSample.Models;

namespace WebApiSample.Dtos;

public class Room : Resource
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public decimal Rate { get; set; }
}