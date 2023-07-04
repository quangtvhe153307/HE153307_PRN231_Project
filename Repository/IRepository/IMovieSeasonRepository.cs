using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.IRepository
{
    public interface IMovieSeasonRepository
    {
        void SaveMovieSeason(MovieSeason movieSeason);
        MovieSeason GetMovieSeasonById(int id);
        void DeleteMovieSeason(MovieSeason movieSeason);
        void UpdateMovieSeason(MovieSeason movieSeason);
        List<MovieSeason> GetMovieSeasons();
    }
}
