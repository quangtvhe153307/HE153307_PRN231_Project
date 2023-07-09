using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using APIProject.DTO.MovieEpisode;

namespace APIProject.DTO.MovieSeason
{
    public class UpdateMovieSeasonRequestDTO
    {
        [Required]
        public int MovieSeasonId { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public DateTime? ReleasedDate { get; set; }
        [Required]
        public bool IsActive { get; set; }
        [Required]
        public int MovieId { get; set; }
        //[Required]
        //public virtual ICollection<CreateMovieEpisodeRequestDTO> MovieEpisodes { get; set; }
    }
}
