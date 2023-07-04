using BusinessObjects;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace APIProject.DTO.Category
{
    public class UpdateCategoryRequestDTO
    {
        [Required]
        public int CategoryId { get; set; }
        [Required]
        public string? CategoryName { get; set; }
    }
}
