namespace CoworkingAPI.Model
{
    public class Space
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public int OwnerId { get; set; }
        public User Owner { get; set; }
        public ICollection<Workplace> Workplaces { get; set; }
    }
}
