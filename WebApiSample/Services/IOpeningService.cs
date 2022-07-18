using WebApiSample.Models;

namespace WebApiSample.Services
{
    public interface IOpeningService
    {
        Task<IEnumerable<Opening>> GetOpeningsAsync();

        Task<IEnumerable<BookingRange>> GetConflictingSlots(
            Guid roomId,
            DateTimeOffset start,
            DateTimeOffset end);
    }
}
