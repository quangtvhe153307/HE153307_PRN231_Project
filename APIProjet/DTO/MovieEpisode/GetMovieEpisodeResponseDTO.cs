using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace APIProject.DTO.MovieEpisode
{
    public class GetMovieEpisodeResponseDTO
    {
        [Key]
        public int EpisodeId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Duration { get; set; }
        public int MovieSeasonId { get; set; }
        public string EpisodeImage { get; set; }
        public double ViewCount { get; set; }
    }
}
