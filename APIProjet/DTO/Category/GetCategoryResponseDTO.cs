using BusinessObjects;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace APIProject.DTO.Category
{
    public class GetCategoryResponseDTO
    {
        [Key]
        public int CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public bool Selected { get; set; }
    }
}
