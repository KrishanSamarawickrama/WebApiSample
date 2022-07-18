using WebApiSample.Models;

namespace WebApiSample.Services
{
    public interface IBookingService
    {
        Task<Booking?> GetBookingAsync(Guid bookingId);

        Task<Guid> CreateBookingAsync(
            Guid userId,
            Guid roomId,
            DateTimeOffset startAt,
            DateTimeOffset endAt);
    }

}
