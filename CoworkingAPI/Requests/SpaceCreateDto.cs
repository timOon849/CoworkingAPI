using System.ComponentModel.DataAnnotations;

namespace CoworkingAPI.Requests
{
    public class SpaceCreateDto
    {
        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        public int OwnerId { get; set; }
    }
}
