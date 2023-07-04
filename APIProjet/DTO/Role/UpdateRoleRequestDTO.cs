using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace APIProject.DTO.Role
{
    public class UpdateRoleRequestDTO
    {
        [Required]
        public int RoleId { get; set; }
        public string RoleName { get; set; }
    }
}
