using BusinessObjects;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace APIProject.DTO.Category
{
    public class CreateCategoryRequestDTO
    {
        [Required]
        public string? CategoryName { get; set; }
    }
}
