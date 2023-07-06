using APIProject.DTO.Movie;
using APIProject.DTO.User;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APIProject.DTO.PurchasedMovie
{
    public class GetPurchasedMovieResponseDTO
    {
        [Key]
        public int UserId { get; set; }
        public int MovieId { get; set; }
        public DateTime? PurchasedTime { get; set; }
        public virtual GetUserResponseDTO User { get; set; }
        public virtual GetMovieResponseDTO Movie { get; set; }
    }
}
