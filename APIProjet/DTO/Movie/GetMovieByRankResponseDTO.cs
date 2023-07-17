using APIProject.DTO.Category;
using APIProject.DTO.MovieSeason;
using System.ComponentModel.DataAnnotations;

namespace APIProject.DTO.Movie
{
    public class GetMovieByRankResponseDTO
    {
        [Key]
        public int MovieId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public bool IsSingleEpisode { get; set; }
        public double Price { get; set; }
        public bool IsFree => Price == 0;
        public bool IsActive { get; set; }
        public string? MovieImage { get; set; }
        public double ViewCount { get; set; }
        public string? TrailerUrl { get; set; }
    }
}
