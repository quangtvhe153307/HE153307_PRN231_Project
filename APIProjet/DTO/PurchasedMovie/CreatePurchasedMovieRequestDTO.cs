using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APIProject.DTO.PurchasedMovie
{
    public class CreatePurchasedMovieRequestDTO
    {
        [Required]
        public int MovieId { get; set; }
    }
}
