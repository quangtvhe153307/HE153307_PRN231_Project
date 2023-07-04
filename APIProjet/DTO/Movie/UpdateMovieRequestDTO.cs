using BusinessObjects;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using APIProject.DTO.Category;
using APIProject.DTO.MovieSeason;

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
        public virtual ICollection<GetCategoryResponseDTO> Categories { get; set; }
        public virtual ICollection<CreateMovieSeasonRequestDTO> MovieSeasons { get; set; }
    }
}
