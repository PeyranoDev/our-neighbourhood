using Domain.Common.Enum;

namespace Domain.Entities
{
    public class Reservation
    {
        public int Id { get; set; }
        public int AmenityId { get; set; }
        public Amenity Amenity { get; set; }
        public DateTime ReservationDate { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public ReservationStatusEnum Status { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public int UserId { get; set; }

        public bool Overlaps(Reservation other)
            => ReservationDate.Date == other.ReservationDate.Date &&
               StartTime < other.EndTime &&
               EndTime > other.StartTime;
    }
}
