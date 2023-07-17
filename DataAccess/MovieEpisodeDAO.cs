using BusinessObjects;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class MovieEpisodeDAO
    {
        public static List<MovieEpisode> GetMovieEpisodes()
        {
            var listMovieEpisodes = new List<MovieEpisode>();
            try
            {
                using (var context = new MyDbContext())
                {
                    listMovieEpisodes = context.MovieEpisodes
                        .ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return listMovieEpisodes;
        }
        public static MovieEpisode FindMovieEpisodeById(int prodId)
        {
            MovieEpisode movieEpisode = null;
            try
            {
                using (var context = new MyDbContext())
                {
                    movieEpisode = context.MovieEpisodes
                        .Include(x => x.MovieSeason)
                        .Include(x => x.MovieViews)
                        .SingleOrDefault(x => x.EpisodeId == prodId);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return movieEpisode;
        }
        public static void SaveMovieEpisode(MovieEpisode movieEpisode)
        {
            try
            {
                using (var context = new MyDbContext())
                {
                    context.MovieEpisodes.Add(movieEpisode);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public static void UpdateMovieEpisode(MovieEpisode movieEpisode)
        {
            try
            {
                using (var context = new MyDbContext())
                {
                    context.Entry<MovieEpisode>(movieEpisode).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public static void DeleteMovieEpisode(MovieEpisode movieEpisode)
        {
            try
            {
                using (var context = new MyDbContext())
                {
                    var p1 = context.MovieEpisodes.SingleOrDefault(x => x.EpisodeId == movieEpisode.EpisodeId);
                    context.MovieEpisodes.Remove(p1);
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
