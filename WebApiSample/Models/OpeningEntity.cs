namespace WebApiSample.Models
{
    public class OpeningEntity : BookingRange
    {
        public Guid RoomId { get; set; }

        public decimal Rate { get; set; }
    }
}
