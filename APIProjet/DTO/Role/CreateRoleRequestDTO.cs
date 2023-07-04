using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace APIProject.DTO.Role
{
    public class CreateRoleRequestDTO
    {
        [Required]
        public string RoleName { get; set; }
    }
}
