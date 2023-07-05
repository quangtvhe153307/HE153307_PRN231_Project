using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APIProject.DTO.MovieRated
{
    public class UpdateMovieRatedRequestDTO
    {
        [Required]
        public int UserId { get; set; }
        [Required]
        public int MovieId { get; set; }
        [Required]
        public int Rated { get; set; }
    }
}
