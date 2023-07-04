using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.IRepository
{
    public interface IMovieRatedRepository
    {
        void SaveMovieRated(MovieRated movieRated);
        double GetMovieAverageRatedById(int movieId);
        void DeleteMovieRated(MovieRated movierated);
        void UpdateMovieRated(MovieRated movierated);
        List<MovieRated> GetMovieRateds();
    }
}
