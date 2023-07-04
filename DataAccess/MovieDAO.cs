﻿using BusinessObjects;
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