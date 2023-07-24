using BusinessObjects;
using DataAccess;
using Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repository
{
    public class CommentRepository : ICommentRepository
    {
        public void SaveComment(Comment comment) => CommentDAO.SaveComment(comment);
        public void UpdateComment(Comment comment) => CommentDAO.UpdateComment(comment);
        public List<Comment> GetComments() => CommentDAO.GetComments();
        public List<Comment> GetCommentByMovieId(int id) => CommentDAO.GetCommentsByMovie(id);

        public void DeleteComment(Comment comment) => CommentDAO.DeleteComment(comment);

        public Comment GetComment(int userId, int movieId, DateTime time) => CommentDAO.GetComment( userId,  movieId, time);
    }
}
