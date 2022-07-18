using Microsoft.AspNetCore.Mvc;
using WebApiSample.Dtos;
using WebApiSample.Models;

namespace WebApiSample.Controllers;

[Route("/")]
[ApiController]
[ApiVersion("1.0")]
public class RootController : ControllerBase
{
    [HttpGet(Name = nameof(GetRoot))]
    public IActionResult GetRoot()
    {
        var output = new RootResponse()
        {
            Self = Link.To(nameof(GetRoot)),
            Rooms = Link.To(nameof(RoomsController.GetAllRooms)), //Url.Link(nameof(RoomsController.GetRooms), null),
            Info = Link.To(nameof(InfoController.GetInfo)) //Url.Link(nameof(InfoController.GetInfo), null)
        };
        return Ok(output);
    }
}