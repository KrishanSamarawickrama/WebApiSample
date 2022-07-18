using Mapster;
using Microsoft.EntityFrameworkCore;
using WebApiSample.Models;
using WebApiSample.Persistence;

namespace WebApiSample.Services
{
    public class DefaultOpeningService : IOpeningService
    {
        private readonly HotelApiDbContext _context;
        private readonly IDateLogicService _dateLogicService;

        public DefaultOpeningService(
            HotelApiDbContext context,
            IDateLogicService dateLogicService)
        {
            _context = context;
            _dateLogicService = dateLogicService;
        }

        public async Task<IEnumerable<Opening>> GetOpeningsAsync()
        {
            var rooms = await _context.Rooms.ToArrayAsync();

            var allOpenings = new List<Opening>();

            foreach (var room in rooms)
            {
                // Generate a sequence of raw opening slots
                var allPossibleOpenings = _dateLogicService.GetAllSlots(
                        DateTimeOffset.UtcNow,
                        _dateLogicService.FurthestPossibleBooking(DateTimeOffset.UtcNow))
                    .ToArray();

                var conflictedSlots = await GetConflictingSlots(
                    room.Id,
                    allPossibleOpenings.First().StartAt,
                    allPossibleOpenings.Last().EndAt);

                // Remove the slots that have conflicts and project
                var openings = allPossibleOpenings
                    .Except(conflictedSlots, new BookingRangeComparer())
                    .Select(slot => new OpeningEntity
                    {
                        RoomId = room.Id,
                        Rate = room.Rate,
                        StartAt = slot.StartAt,
                        EndAt = slot.EndAt
                    })
                    .Select(model => model.Adapt<Opening>());

                allOpenings.AddRange(openings);
            }

            return allOpenings;
        }

        public async Task<IEnumerable<BookingRange>> GetConflictingSlots(
            Guid roomId,
            DateTimeOffset start,
            DateTimeOffset end)
        {
            var entities = await _context.Bookings.ToListAsync();
            return entities
                .Where(b => b?.Room?.Id == roomId && _dateLogicService.DoesConflict(b, start, end))
                // Split each existing booking up into a set of atomic slots
                .SelectMany(existing => _dateLogicService
                    .GetAllSlots(existing.StartAt, existing.EndAt))
                .ToArray();
        }
    }
}
