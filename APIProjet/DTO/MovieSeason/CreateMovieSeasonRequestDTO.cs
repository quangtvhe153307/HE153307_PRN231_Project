using BusinessObjects;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using APIProject.DTO.MovieEpisode;

namespace APIProject.DTO.MovieSeason
{
    public class CreateMovieSeasonRequestDTO
    {
        [Required]
        public int MovieId { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public DateTime? ReleasedDate { get; set; }
        public virtual ICollection<CreateMovieEpisodeRequestDTO> MovieEpisodes { get; set; }
    }
}
