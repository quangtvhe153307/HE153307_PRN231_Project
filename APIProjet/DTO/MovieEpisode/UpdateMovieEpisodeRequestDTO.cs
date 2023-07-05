using System.ComponentModel.DataAnnotations;

namespace APIProject.DTO.MovieEpisode
{
    public class UpdateMovieEpisodeRequestDTO
    {
        [Required]
        public int EpisodeId { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public double Duration { get; set; }
        [Required]
        public int MovieSeasonId { get; set; }
    }
}
