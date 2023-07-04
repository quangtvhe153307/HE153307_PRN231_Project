using BusinessObjects;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APIProject.DTO.Comment
{
    public class UpdateCommentRequestDTO
    {
        [Required]
        public int UserId { get; set; }
        [Required]
        public int MovieId { get; set; }
        public DateTime CommentedDate { get; set; }
        public string Content { get; set; }
    }
}
