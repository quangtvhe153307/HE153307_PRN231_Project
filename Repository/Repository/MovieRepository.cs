﻿using BusinessObjects;
using DataAccess;
using Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repository
{
    public class MovieRepository : IMovieRepository
    {
        public void SaveMovie(Movie movie) => MovieDAO.SaveMovie(movie);
        public void UpdateMovie(Movie movie) => MovieDAO.UpdateMovie(movie);
        public List<Movie> GetMovies() => MovieDAO.GetMovies();
        public Movie GetMovieById(int id) => MovieDAO.FindMovieById(id);

        public void DeleteMovie(Movie movie) => MovieDAO.DeleteMovie(movie);

        public List<Movie> GetMoviesByRank(DateTime startDate, DateTime endDate) => MovieDAO.GetMoviesByRank(startDate, endDate);

        public bool IsPurchased(int userId, int movieId) => MovieDAO.IsPurchased(userId, movieId);

        public bool CheckFreeMovie(int id) => MovieDAO.CheckFreeMovie(id);
    }
}
