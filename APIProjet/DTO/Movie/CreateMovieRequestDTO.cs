using BusinessObjects;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using APIProject.DTO.Category;
using APIProject.DTO.MovieSeason;
using APIProject.DTO.CategoryMovie;

namespace APIProject.DTO.Movie
{
    public class CreateMovieRequestDTO
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public DateTime? ReleaseDate { get; set; }
        [Required]
        public bool IsSingleEpisode { get; set; }
        [Required]
        public double Price { get; set; }
        public bool IsFree => Price == 0;
        public string? MovieImage { get; set; }
        public string? TrailerUrl { get; set; }
        [Required]
        public virtual ICollection<CreateCategoryMovieRequestDTO> Categories { get; set; }
        [Required]
        public virtual ICollection<CreateMovieSeasonRequestDTO> MovieSeasons { get; set; }
    }
}
