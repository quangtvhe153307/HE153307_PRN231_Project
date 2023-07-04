using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace APIProject.DTO.Role
{
    public class GetRoleResponseDTO
    {
        [Key]
        public int RoleId { get; set; }
        public string RoleName { get; set; }
    }
}
