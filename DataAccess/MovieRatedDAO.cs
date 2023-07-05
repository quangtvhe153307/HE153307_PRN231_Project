using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class MovieratedDAO
    {
        public static List<MovieRated> GetMovierateds()
        {
            var listMovierateds = new List<MovieRated>();
            try
            {
                using var context = new MyDbContext();
                listMovierateds = context.MovieRateds
                    .ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return listMovierateds;
        }
        public static double FindMovieAverageRatedById(int movieId)
        {
            double rated = 0;
            try
            {
                using (var context = new MyDbContext())
                {
                    rated = context.MovieRateds
                        .Where(x => x.MovieId == movieId)
                        .Average(x => x.Rated);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return rated;
        }
        public static void SaveMovierated(MovieRated movierated)
        {
            try
            {
                using (var context = new MyDbContext())
                {
                    context.MovieRateds.Add(movierated);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public static void UpdateMovierated(MovieRated movierated)
        {
            try
            {
                using (var context = new MyDbContext())
                {
                    context.Entry<MovieRated>(movierated).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public static void DeleteMovierated(MovieRated movierated)
        {
            try
            {
                using (var context = new MyDbContext())
                {
                    var p1 = context.MovieRateds.SingleOrDefault(x => x.MovieId == movierated.MovieId && x.UserId == movierated.UserId);
                    context.MovieRateds.Remove(p1);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public static MovieRated GetMovieRated(int movieId, int userId)
        {
            MovieRated movieRated = null;
            try
            {
                using var context = new MyDbContext();
                movieRated = context.MovieRateds
                    .SingleOrDefault(x => x.MovieId == movieId && x.UserId == userId);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return movieRated;
        }
    }
}
