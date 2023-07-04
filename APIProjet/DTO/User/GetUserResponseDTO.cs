using BusinessObjects;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using APIProject.DTO.Role;

namespace APIProject.DTO.User
{
    public class GetUserResponseDTO
    {
        [Key]
        public int UserId { get; set; }
        public string Email { get; set; }
        public int RoleId { get; set; }
        public virtual GetRoleResponseDTO Role { get; set; }
        public virtual ICollection<PurchasedMovie> PurchasedMovies { get; set; }
    }
}
