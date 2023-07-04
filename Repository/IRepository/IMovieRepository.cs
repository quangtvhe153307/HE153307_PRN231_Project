using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.IRepository
{
    public interface IMovieRepository
    {
        void SaveMovie(Movie movie);
        Movie GetMovieById(int id);
        void DeleteMovie(Movie movie);
        void UpdateMovie(Movie movie);
        List<Movie> GetMovies();
    }
}
