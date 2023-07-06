using System.ComponentModel.DataAnnotations.Schema;

namespace APIProject.DTO.PurchasedMovie
{
    public class UpdatePurchasedMovieRequestDTO
    {
        public int UserId { get; set; }
        public int MovieId { get; set; }
        public DateTime? PurchasedTime { get; set; }
    }
}
