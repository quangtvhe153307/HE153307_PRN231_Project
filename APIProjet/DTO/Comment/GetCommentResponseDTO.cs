using APIProject.DTO.Movie;
using APIProject.DTO.User;
using BusinessObjects;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APIProject.DTO.Comment
{
    public class GetCommentResponseDTO
    {
        [Key]
        public int UserId { get; set; }
        public int MovieId { get; set; }
        public DateTime CommentedDate { get; set; }
        public TimeSpan CommentedTimeInterval => DateTime.Now - CommentedDate;
        public string Content { get; set; }
        public virtual GetUserResponseDTO User { get; set; }
        public virtual GetMovieResponseDTO Movie { get; set; }
    }
}
