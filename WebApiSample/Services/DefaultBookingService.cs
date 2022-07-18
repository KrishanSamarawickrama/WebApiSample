using Mapster;
using Microsoft.EntityFrameworkCore;
using WebApiSample.Models;
using WebApiSample.Persistence;

namespace WebApiSample.Services
{
    public class DefaultBookingService : IBookingService
    {
        private readonly HotelApiDbContext _context;
        private readonly IDateLogicService _dateLogicService;

        public DefaultBookingService(
            HotelApiDbContext context,
            IDateLogicService dateLogicService
        )
        {
            _context = context;
            _dateLogicService = dateLogicService;
        }

        public Task<Guid> CreateBookingAsync(
            Guid userId,
            Guid roomId,
            DateTimeOffset startAt,
            DateTimeOffset endAt)
        {
            // TODO: Save the new booking to the database
            throw new NotImplementedException();
        }

        public async Task<Booking?> GetBookingAsync(Guid bookingId)
        {
            var entity = await _context.Bookings
                .SingleOrDefaultAsync(b => b.Id == bookingId);

            if (entity == null) return null;

            return entity.Adapt<Booking>();
        }
    }
}