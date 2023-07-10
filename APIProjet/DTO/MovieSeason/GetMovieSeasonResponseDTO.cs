using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using APIProject.DTO.MovieEpisode;

namespace APIProject.DTO.MovieSeason
{
    public class GetMovieSeasonResponseDTO
    {
        [Key]
        public int MovieSeasonId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime? ReleasedDate { get; set; }
        public bool IsActive { get; set; }
        public int MovieId { get; set; }
        public virtual ICollection<GetMovieEpisodeResponseDTO> MovieEpisodes { get; set; }
        public double ViewCount { get; set; }
    }
}
