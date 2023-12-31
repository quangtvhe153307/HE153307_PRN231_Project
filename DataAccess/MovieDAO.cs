﻿using BusinessObjects;
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
                    //listMovies = context.Movies
                    //    .Include(x => x.Categories)
                    //    .Include(x => x.MovieSeasons)
                    //    .ThenInclude(x => x.MovieEpisodes)
                    //    .ThenInclude(x => x.MovieViews.Where(mv => mv.ViewedDate >= startDate && mv.ViewedDate < endDate))
                    //    .ToList();

                    listMovies = context.Movies
                        .Select(x => new Movie
                        {
                            MovieId = x.MovieId,
                            Title = x.Title,
                            Description = x.Description,
                            ReleaseDate = x.ReleaseDate,
                            IsSingleEpisode = x.IsSingleEpisode,
                            Price = x.Price,
                            IsActive = x.IsActive,
                            MovieImage = x.MovieImage,
                            UpdatedDate = x.UpdatedDate,
                            TrailerUrl = x.TrailerUrl,
                            MovieSeasons = x.MovieSeasons.Select(season => new MovieSeason
                            {
                                MovieSeasonId = season.MovieSeasonId,
                                Title = season.Title,
                                Description = season.Description,
                                ReleasedDate = season.ReleasedDate,
                                IsActive = season.IsActive,
                                MovieId = season.MovieId,
                                MovieEpisodes = season.MovieEpisodes
                                                .Where(episode => episode.MovieViews
                                                                        .Any(mv => mv.ViewedDate >= startDate && mv.ViewedDate < endDate))
                                                .Select(me => new MovieEpisode
                                                {
                                                    EpisodeId = me.EpisodeId,
                                                    Title = me.Title,
                                                    Description = me.Description,
                                                    Duration = me.Duration,
                                                    EpisodeImage = me.EpisodeImage,
                                                    MovieSeasonId = me.MovieSeasonId,
                                                    UrlSource = me.UrlSource,
                                                    MovieViews = me.MovieViews.Where(mv => mv.ViewedDate >= startDate && mv.ViewedDate < endDate).ToList()
                                                })
                                                .ToList()
                            }).ToList()
                        }).ToList();
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
        public static bool IsPurchased(int userId, int movieId)
        {
            try
            {
                using (var context = new MyDbContext())
                {
                    var p1 = context.PurchasedMovies.Any(x => x.UserId == userId && x.MovieId == movieId);
                    return p1;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public static bool CheckFreeMovie(int id)
        {
            try
            {
                using (var context = new MyDbContext())
                {
                    var p1 = context.Movies.SingleOrDefault(x => x.MovieId == id);
                    if(p1 == null)
                    {
                        return false;
                    }
                    return p1.IsFree;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
