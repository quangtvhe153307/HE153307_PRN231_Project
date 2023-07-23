using System.ComponentModel.DataAnnotations;

namespace APIProject.Controllers
{
    public class UserChangePasswordRequestDTO
    {
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }
    }
}
