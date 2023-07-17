using BusinessObjects;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using APIProject.DTO.Category;
using APIProject.DTO.MovieSeason;
using APIProject.DTO.CategoryMovie;

namespace APIProject.DTO.Movie
{
    public class UpdateMovieRequestDTO
    {
        [Required]
        public int MovieId { get; set; }
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
        public bool IsActive { get; set; }
        public string? MovieImage { get; set; }
        public string? TrailerUrl { get; set; }
        //public virtual ICollection<CreateCategoryMovieRequestDTO> Categories { get; set; }
    }
}
