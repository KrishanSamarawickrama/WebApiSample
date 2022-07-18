using System.Collections.ObjectModel;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiSample.Dtos;
using WebApiSample.Models;
using WebApiSample.Persistence;
using WebApiSample.Services;

namespace WebApiSample.Controllers;

[Route("/[controller]")]
[ApiController]
public class RoomsController : ControllerBase
{
    private readonly IRoomService _service;
    private readonly IOpeningService _openingService;

    public RoomsController(IRoomService service, IOpeningService openingService)
    {
        _service = service;
        _openingService = openingService;
    }
    
    [HttpGet(Name = nameof(GetAllRooms))]
    [ProducesResponseType(200)]
    public async Task<ActionResult<ResourceCollection<Room>>> GetAllRooms()
    {
        var rooms = await _service.GetRoomsAsync();
        var collection = new ResourceCollection<Room>
        {
            Self = Link.ToCollection(nameof(GetAllRooms)),
            Value = rooms.ToArray()
        };
        return Ok(collection);
    }
    
    [HttpGet("openings", Name = nameof(GetAllRoomOpenings))]
    [ProducesResponseType(200)]
    public async Task<ActionResult<ResourceCollection<Opening>>> GetAllRoomOpenings()
    {
        var openings = await _openingService.GetOpeningsAsync();

        var collection = new ResourceCollection<Opening>()
        {
            Self = Link.ToCollection(nameof(GetAllRoomOpenings)),
            Value = openings.ToArray()
        };

        return collection;
    }

    [HttpGet("{id}", Name = nameof(GetRoomById))]
    [ProducesResponseType(404)]
    [ProducesResponseType(200)]
    public async Task<ActionResult<Room>> GetRoomById(Guid id)
    {
        var output = await _service.GetRoomAsync(id);
        if (output == null) return NotFound();
        return Ok(output);
    }
}