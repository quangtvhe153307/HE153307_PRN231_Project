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
    public class MovieSeasonRepository : IMovieSeasonRepository
    {
        public void SaveMovieSeason(MovieSeason movieseason) => MovieSeasonDAO.SaveMovieSeason(movieseason);
        public void UpdateMovieSeason(MovieSeason movieseason) => MovieSeasonDAO.UpdateMovieseason(movieseason);
        public List<MovieSeason> GetMovieSeasons() => MovieSeasonDAO.GetMovieSeasons();
        public MovieSeason GetMovieSeasonById(int id) => MovieSeasonDAO.FindMovieSeasonById(id);
        public void DeleteMovieSeason(MovieSeason movieseason) => MovieSeasonDAO.DeleteMovieseason(movieseason);
    }
}
