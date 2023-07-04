using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using APIProject.DTO.MovieEpisode;

namespace APIProject.DTO.MovieSeason
{
    public class UpdateMovieSeasonRequestDTO
    {
        [Required]
        public int MovieSeasonId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime? ReleasedDate { get; set; }
        [Required]
        public int MovieId { get; set; }
        [Required]
        public virtual ICollection<CreateMovieEpisodeRequestDTO> MovieEpisodes { get; set; }
    }
}
