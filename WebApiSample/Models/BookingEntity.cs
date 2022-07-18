﻿namespace WebApiSample.Models
{
    public class BookingEntity : BookingRange
    {
        public Guid Id { get; set; }

        public RoomEntity? Room { get; set; }

        public DateTimeOffset CreatedAt { get; set; }

        public DateTimeOffset ModifiedAt { get; set; }

        public decimal Total { get; set; }
    }
}
