using System.ComponentModel.DataAnnotations;

namespace APIProject.DTO.CategoryMovie
{
    public class CreateCategoryMovieRequestDTO
    {
        [Required]
        public int CategoryId { get; set; }
    }
}
