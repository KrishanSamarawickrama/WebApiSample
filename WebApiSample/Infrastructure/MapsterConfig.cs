using Mapster;
using WebApiSample.Controllers;
using WebApiSample.Dtos;
using WebApiSample.Models;

namespace WebApiSample.Infrastructure;

public static class MapsterConfig
{
    public static void Configure()
    {
        TypeAdapterConfig.GlobalSettings.NewConfig<RoomEntity, Room>()
            .Map(dest => dest.Self, src => Link.To(nameof(RoomsController.GetRoomById), new {id = src.Id}))
            .Compile();

        TypeAdapterConfig.GlobalSettings.NewConfig<OpeningEntity, Opening>()
            .Map(dest => dest.Room, src => Link.To(nameof(RoomsController.GetRoomById), new {id = src.RoomId}))
            .Compile();

        TypeAdapterConfig.GlobalSettings.NewConfig<BookingEntity, Booking>()
            .Map(dest => dest.Self, src => Link.To(nameof(BookingsController.GetBookingById), new {bookingId = src.Id}))
            .Map(dest => dest.Room, src => Link.To(nameof(RoomsController.GetRoomById), new {id = src.Id}))
            .Compile();
    }
}