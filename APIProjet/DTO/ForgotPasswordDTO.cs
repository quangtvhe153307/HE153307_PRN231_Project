using System.ComponentModel.DataAnnotations;

namespace APIProject.DTO
{
    public class ForgotPasswordDTO
    {
        [Required]
        public string Email { get; set; }
    }
}
