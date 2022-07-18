using WebApiSample.Models;

namespace WebApiSample.Dtos;

public class RootResponse : Resource
{
    public Link? Rooms { get; set; }
    public Link? Info { get; set; }
}