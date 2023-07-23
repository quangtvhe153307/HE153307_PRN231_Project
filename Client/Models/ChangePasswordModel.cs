using System.ComponentModel.DataAnnotations;

namespace Client.Models
{
    public class ChangePasswordModel
    {
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Compare(nameof(NewPassword), ErrorMessage = "The new password and new confirmation password do not match.")]
        public string ConfirmNewPassword { get; set; }
    }
}
