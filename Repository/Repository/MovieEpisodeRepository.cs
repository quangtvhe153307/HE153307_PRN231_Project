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
    public class MovieEpisodeRepository : IMovieEpisodeRepository
    {
        public void SaveMovieEpisode(MovieEpisode movieepisode) => MovieEpisodeDAO.SaveMovieEpisode(movieepisode);
        public void UpdateMovieEpisode(MovieEpisode movieepisode) => MovieEpisodeDAO.UpdateMovieEpisode(movieepisode);
        public List<MovieEpisode> GetMovieEpisodes() => MovieEpisodeDAO.GetMovieEpisodes();
        public MovieEpisode GetMovieEpisodeById(int id) => MovieEpisodeDAO.FindMovieEpisodeById(id);

        public void DeleteMovieEpisode(MovieEpisode movieepisode) => MovieEpisodeDAO.DeleteMovieEpisode(movieepisode);

        public MovieEpisode GetMovieSourceById(int id) => MovieEpisodeDAO.FindMovieEpisodeByIdNotInclludeView(id);

        public void AddMovieView(MovieView movieview) => MovieEpisodeDAO.AddMovieView(movieview);
    }   
}
