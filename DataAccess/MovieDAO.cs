using BusinessObjects;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class MovieDAO
    {
        public static List<Movie> GetMovies()
        {
            var listMovies = new List<Movie>();
            try
            {
                using (var context = new MyDbContext())
                {
                    listMovies = context.Movies
                        .Include(x => x.Categories)
                        .Include(x => x.MovieSeasons)
                        .ThenInclude(x => x.MovieEpisodes)
                        .ThenInclude(x => x.MovieViews)
                        .ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return listMovies;
        }        
        public static List<Movie> GetMoviesByRank(DateTime startDate, DateTime endDate)
        {
            var listMovies = new List<Movie>();
            try
            {
                using (var context = new MyDbContext())
                {
                    listMovies = context.Movies
                        .Include(x => x.Categories)
                        .Include(x => x.MovieSeasons)
                        .ThenInclude(x => x.MovieEpisodes)
                        .ThenInclude(x => x.MovieViews.Where(mv => mv.ViewedDate >= startDate && mv.ViewedDate < endDate))
                        .ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return listMovies;
        }
        public static Movie FindMovieById(int prodId)
        {
            Movie movie = null;
            try
            {
                using (var context = new MyDbContext())
                {
                    movie = context.Movies
                        .Include(x => x.Categories)
                        .Include(x => x.MovieSeasons)
                        .ThenInclude(x => x.MovieEpisodes)
                        .ThenInclude(x => x.MovieViews)
                        .SingleOrDefault(x => x.MovieId == prodId);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return movie;
        }
        public static void SaveMovie(Movie movie)
        {
            try
            {
                using (var context = new MyDbContext())
                {
                    List<Category> categories = (List<Category>)movie.Categories;
                    List<int> categoryIds = new List<int>();
                    foreach (var item in categories)
                    {
                        categoryIds.Add(item.CategoryId);
                    }
                    List<Category> categoryFromDb = context.Categories.Where(c => categoryIds.Contains(c.CategoryId)).ToList();

                    movie.Categories = categoryFromDb;
                    context.Movies.Add(movie);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public static void UpdateMovie(Movie movie)
        {
            try
            {
                using (var context = new MyDbContext())
                {
                    context.Entry<Movie>(movie).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public static void DeleteMovie(Movie movie)
        {
            try
            {
                using (var context = new MyDbContext())
                {
                    var p1 = context.Movies.SingleOrDefault(x => x.MovieId == movie.MovieId);
                    context.Movies.Remove(p1);
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
