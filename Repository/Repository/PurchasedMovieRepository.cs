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
    public class PurchasedMovieRepository : IPurchasedMovieRepository
    {
        public void SavePurchasedMovies(PurchasedMovie purchasedMovies) => PurchasedMovieDAO.SavePurchasedMovies(purchasedMovies);
        public void UpdatePurchasedMovies(PurchasedMovie purchasedMovies) => PurchasedMovieDAO.UpdatePurchasedMovies(purchasedMovies);
        public List<PurchasedMovie> GetPurchasedMovies() => PurchasedMovieDAO.GetPurchasedMovies();
        public List<PurchasedMovie> GetPurchasedMoviesById(int userId) => PurchasedMovieDAO.FindPurchasedMoviesById(userId);
        public void DeletePurchasedMovies(PurchasedMovie purchasedmovies) => PurchasedMovieDAO.DeletePurchasedMovies(purchasedmovies);
    }
}
