namespace CoworkingAPI.Model
{
    public class Workplace
    {
        public int Id { get; set; }
        public int SpaceId { get; set; }
        public Space Space { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal PricePerHour { get; set; }
        public int Capacity { get; set; }
        public ICollection<Booking> Bookings { get; set; }
    }
}
