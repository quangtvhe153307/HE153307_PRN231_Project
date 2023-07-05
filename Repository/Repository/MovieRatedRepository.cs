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
    public class MovieRatedRepository : IMovieRatedRepository
    {
        public void SaveMovieRated(MovieRated movierated) => MovieratedDAO.SaveMovierated(movierated);
        public void UpdateMovieRated(MovieRated movierated) => MovieratedDAO.UpdateMovierated(movierated);
        public List<MovieRated> GetMovieRateds() => MovieratedDAO.GetMovierateds();
        public double GetMovieAverageRatedById(int movieId) => MovieratedDAO.FindMovieAverageRatedById(movieId);

        public void DeleteMovieRated(MovieRated movierated) => MovieratedDAO.DeleteMovierated(movierated);

        public MovieRated GetMovieRated(int movieId, int userId) => MovieratedDAO.GetMovieRated(movieId,userId);
    }
}
