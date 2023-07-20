using BusinessObjects;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class CommentDAO
    {
        public static List<Comment> GetComments()
        {
            var listComments = new List<Comment>();
            try
            {
                using (var context = new MyDbContext())
                {
                    listComments = context.Comments
                        .Include(x => x.User)
                        .ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return listComments;
        }
        public static List<Comment> GetCommentsByMovie(int movieId)
        {
            List<Comment> comments = null;
            try
            {
                using (var context = new MyDbContext())
                {
                    comments = context.Comments
                        .Include(x => x.User)
                        .Where(x => x.MovieId == movieId)
                        .ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return comments;
        }
        public static void SaveComment(Comment comment)
        {
            try
            {
                using (var context = new MyDbContext())
                {
                    context.Comments.Add(comment);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public static void UpdateComment(Comment comment)
        {
            try
            {
                using (var context = new MyDbContext())
                {
                    context.Entry<Comment>(comment).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public static void DeleteComment(Comment comment)
        {
            try
            {
                using (var context = new MyDbContext())
                {
                    var p1 = context.Comments.SingleOrDefault(x => x.MovieId == comment.MovieId && x.UserId == comment.UserId && x.CommentedDate == comment.CommentedDate);
                    context.Comments.Remove(p1);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
