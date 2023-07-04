using BusinessObjects;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace APIProject.DTO.User
{
    public class UpdateUserRequestDTO
    {
        public int UserId { get; set; }
        [EmailAddress]
        public string Email { get; set; }
    }
}
