using BusinessObjects;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APIProject.DTO.Comment
{
    public class CreateCommentRequestDTO
    {
        [Required]
        public int UserId { get; set; }
        [Required]
        public int MovieId { get; set; }
        [Required]
        public string Content { get; set; }
    }
}
