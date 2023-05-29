using System.ComponentModel.DataAnnotations;

namespace NzWalkWebApi.Models.DTO
{
    public class UpdateWalkRequestDto
    {

        [Required]
        [MaxLength(255)]
        public string Name { get; set; }
        [Required]
        [MaxLength(255)]
        public string Description { get; set; }
        [Required]
        [Range(0, 50)]
        public double LengthInKm { get; set; }

        public string? WalkImageUrl { get; set; }
        [Required]
        public Guid DifficultyId { get; set; }
        [Required]
        public Guid RegionID { get; set; }
    }
}
