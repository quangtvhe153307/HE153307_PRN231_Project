using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.IRepository
{
    public interface ICommentRepository
    {
        void SaveComment(Comment comment);
        List<Comment> GetCommentByMovieId(int movieId);
        void DeleteComment(Comment comment);
        void UpdateComment(Comment comment);
        List<Comment> GetComments();
        Comment GetComment(int userId, int movieId, DateTime time);
    }
}
