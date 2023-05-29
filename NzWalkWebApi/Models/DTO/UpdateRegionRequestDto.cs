using System.ComponentModel.DataAnnotations;

namespace NzWalkWebApi.Models.DTO
{
    public class UpdateRegionRequestDto
    {
        [Required]
        [MinLength(3, ErrorMessage = "Code has to be a minimum of 3 character")]
        [MaxLength(3, ErrorMessage = "Code has to be a maximum of 3 character")]
        public string Code { get; set; }
        [Required]
        [MinLength(3, ErrorMessage = "Code has to be a minimum of 3 character")]
        [MaxLength(100, ErrorMessage = "Code has to be a maximum of 100 character")]
        public string Name { get; set; }
        public string? RegionImageUrl { get; set; }
    }
}
