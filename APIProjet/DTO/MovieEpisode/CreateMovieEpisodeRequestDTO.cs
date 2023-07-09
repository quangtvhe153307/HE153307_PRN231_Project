using System.ComponentModel.DataAnnotations;

namespace APIProject.DTO.MovieEpisode
{
    public class CreateMovieEpisodeRequestDTO
    {
        [Required]
        public int MovieSeasonId { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string Duration { get; set; }
        [Required]
        public string EpisodeImage { get; set; }
    }
}
