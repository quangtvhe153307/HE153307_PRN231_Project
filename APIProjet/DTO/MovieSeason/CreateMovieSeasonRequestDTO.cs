using BusinessObjects;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using APIProject.DTO.MovieEpisode;

namespace APIProject.DTO.MovieSeason
{
    public class CreateMovieSeasonRequestDTO
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime? ReleasedDate { get; set; }
        public virtual ICollection<CreateMovieEpisodeRequestDTO> MovieEpisodes { get; set; }
    }
}
