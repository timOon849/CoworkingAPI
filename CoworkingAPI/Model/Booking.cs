namespace CoworkingAPI.Model
{
    public class Booking
    {
        public int Id { get; set; }
        public int WorkplaceId { get; set; }
        public Workplace Workplace { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Status { get; set; } = "Pending";
    }
}
