using BusinessObjects;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace APIProject.DTO.User
{
    public class CreateUserRequestDTO
    {
        [EmailAddress]
        public string Email { get; set; }
        public string Password { get; set; }
        public int RoleId { get; set; }
    }
}
