using BusinessObjects;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using APIProject.DTO.Role;
using APIProject.DTO.PurchasedMovie;

namespace APIProject.DTO.User
{
    public class GetUserResponseDTO
    {
        [Key]
        public int UserId { get; set; }
        public string Email { get; set; }
        public int RoleId { get; set; }
        public virtual GetRoleResponseDTO Role { get; set; }
        public virtual ICollection<GetPurchasedMovieResponseDTO> PurchasedMovies { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool EmailConfirmed { get; set; }
        public double Balance { get; set; }
        public DateTime? ExpirationDate { get; set; }
    }
}
