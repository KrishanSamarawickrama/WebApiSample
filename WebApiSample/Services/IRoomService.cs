using WebApiSample.Dtos;

namespace WebApiSample.Services;

public interface IRoomService
{
    Task<Room?> GetRoomAsync(Guid id);
    Task<ICollection<Room>> GetRoomsAsync();
}