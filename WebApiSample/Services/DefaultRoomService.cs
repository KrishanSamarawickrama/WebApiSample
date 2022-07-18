using Mapster;
using Microsoft.EntityFrameworkCore;
using WebApiSample.Dtos;
using WebApiSample.Persistence;

namespace WebApiSample.Services;

public class DefaultRoomService : IRoomService
{
    private readonly HotelApiDbContext _dbContext;

    public DefaultRoomService(HotelApiDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Room?> GetRoomAsync(Guid id)
    {
        var entity = await _dbContext.Rooms.SingleOrDefaultAsync(x => x.Id == id);
        return entity?.Adapt<Room>();
    }

    public async Task<ICollection<Room>> GetRoomsAsync()
    {
        var entities = await _dbContext.Rooms.ToListAsync();
        return entities.Adapt<List<Room>>();
    }
}