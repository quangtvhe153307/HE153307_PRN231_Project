using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.IRepository
{
    public interface IPurchasedMoviesRepository
    {
        void SavePurchasedMovies(PurchasedMovie purchasedMovies);
        List<PurchasedMovie> GetPurchasedMoviesById(int userId);
        void DeletePurchasedMovies(PurchasedMovie purchasedMovies);
        void UpdatePurchasedMovies(PurchasedMovie purchasedMovies);
        List<PurchasedMovie> GetPurchasedMoviess();
    }
}
