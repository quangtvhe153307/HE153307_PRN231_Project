using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APIProject.DTO.MovieRated
{
    public class GetMovieRatedResponseDTO
    {
        public int UserId { get; set; }
        [Key]
        public int MovieId { get; set; }
        public int Rated { get; set; }
        public DateTime RatedTime { get; set; }
    }
}
