using WebApiSample.Models;

namespace WebApiSample.Dtos;

public class HotelInfo : Resource
{
    public string Title { get; set; } = default!;
    public string Tagline { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string Website { get; set; } = default!;
    public Address Location { get; set; } = default!;
}

public class Address
{
    public string Street { get; set; } = default!;
    public string City { get; set; } = default!;
    public string Country { get; set; } = default!;
}