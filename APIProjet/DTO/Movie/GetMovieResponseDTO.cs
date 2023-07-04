using BusinessObjects;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using APIProject.DTO.Category;
using APIProject.DTO.MovieSeason;

namespace APIProject.DTO.Movie
{
    public class GetMovieResponseDTO
    {
        [Key]
        public int MovieId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public bool IsSingleEpisode { get; set; }
        public double Price { get; set; }
        public bool IsFree => Price == 0;
        public virtual ICollection<GetCategoryResponseDTO> Categories { get; set; }
        public virtual ICollection<GetMovieSeasonResponseDTO> MovieSeasons { get; set; }
    }
}
