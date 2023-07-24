using BusinessObjects;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace APIProject.DTO.User
{
    public class AddUserDTO
    {
        [EmailAddress]
        public string Email { get; set; }
        public int RoleId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool EmailConfirmed { get; set; }
        public double Balance { get; set; }
        public DateTime? ExpirationDate { get; set; }
    }
}
