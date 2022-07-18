using Microsoft.OpenApi.Writers;
using WebApiSample.Models;
using WebApiSample.Services;

namespace WebApiSample.Persistence;

public static class SeedData
{
    public static async Task InitializeAsync(WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        await AddTestData(scope.ServiceProvider.GetRequiredService<HotelApiDbContext>(),
            scope.ServiceProvider.GetRequiredService<IDateLogicService>());
    }
    
    private static async Task AddTestData(HotelApiDbContext context,IDateLogicService dateLogicService)
    {
        if(context.Rooms.Any()) return;

        var oxford = new RoomEntity
        {
            Id = Guid.Parse("301df04d-8679-4b1b-ab92-0a586ae53d08"),
            Name = "Oxford Suite",
            Rate = 10119,
        };
        context.Rooms.Add(oxford);

        context.Rooms.Add(new RoomEntity
        {
            Id = Guid.Parse("ee2b83be-91db-4de5-8122-35a9e9195976"),
            Name = "Driscoll Suite",
            Rate = 23959
        });
        
        var today = DateTimeOffset.Now;
        var start = dateLogicService.AlignStartTime(today);
        var end = start.Add(dateLogicService.GetMinimumStay());

        context.Bookings.Add(new BookingEntity
        {
            Id = Guid.Parse("2eac8dea-2749-42b3-9d21-8eb2fc0fd6bd"),
            Room = oxford,
            CreatedAt = DateTimeOffset.UtcNow,
            StartAt = start,
            EndAt = end,
            Total = oxford.Rate,
        });

        await context.SaveChangesAsync();

    }
}