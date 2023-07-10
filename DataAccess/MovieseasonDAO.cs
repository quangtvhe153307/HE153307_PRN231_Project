using BusinessObjects;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class MovieSeasonDAO
    {
        public static List<MovieSeason> GetMovieSeasons()
        {
            var listMovieSeasons = new List<MovieSeason>();
            try
            {
                using (var context = new MyDbContext())
                {
                    listMovieSeasons = context.MovieSeasons
                        .ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return listMovieSeasons;
        }
        public static MovieSeason FindMovieSeasonById(int prodId)
        {
            MovieSeason movieseason = null;
            try
            {
                using (var context = new MyDbContext())
                {
                    movieseason = context.MovieSeasons
                        .Include(x => x.MovieEpisodes)
                        .ThenInclude(x => x.MovieViews)
                        .SingleOrDefault(x => x.MovieSeasonId == prodId);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return movieseason;
        }
        public static void SaveMovieSeason(MovieSeason movieseason)
        {
            try
            {
                using (var context = new MyDbContext())
                {
                    context.MovieSeasons.Add(movieseason);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public static void UpdateMovieseason(MovieSeason movieseason)
        {
            try
            {
                using (var context = new MyDbContext())
                {
                    context.Entry<MovieSeason>(movieseason).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public static void DeleteMovieseason(MovieSeason movieseason)
        {
            try
            {
                using (var context = new MyDbContext())
                {
                    var p1 = context.MovieSeasons.SingleOrDefault(x => x.MovieSeasonId == movieseason.MovieSeasonId);
                    context.MovieSeasons.Remove(p1);
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
