using BusinessObjects;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class PurchasedMovieDAO
    {
        public static List<PurchasedMovie> GetPurchasedMovies()
        {
            var listPurchasedmoviess = new List<PurchasedMovie>();
            try
            {
                using (var context = new MyDbContext())
                {
                    listPurchasedmoviess = context.PurchasedMovies
                        .Include(x => x.Movie)
                        .ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return listPurchasedmoviess;
        }
        public static List<PurchasedMovie> FindPurchasedMoviesById(int useId)
        {
            List<PurchasedMovie> purchasedmovies = null;
            try
            {
                using (var context = new MyDbContext())
                {
                    purchasedmovies = context.PurchasedMovies
                        .Where(x => x.UserId == useId)
                        .ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return purchasedmovies;
        }
        public static void SavePurchasedMovies(PurchasedMovie purchasedmovies)
        {
            try
            {
                using (var context = new MyDbContext())
                {
                    context.PurchasedMovies.Add(purchasedmovies);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public static void UpdatePurchasedMovies(PurchasedMovie purchasedmovies)
        {
            try
            {
                using (var context = new MyDbContext())
                {
                    context.Entry<PurchasedMovie>(purchasedmovies).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public static void DeletePurchasedMovies(PurchasedMovie purchasedmovies)
        {
            try
            {
                using (var context = new MyDbContext())
                {
                    var p1 = context.PurchasedMovies.SingleOrDefault(x => x.MovieId == purchasedmovies.MovieId && x.UserId == purchasedmovies.UserId);
                    context.PurchasedMovies.Remove(p1);
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
