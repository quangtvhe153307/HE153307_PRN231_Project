using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.IRepository
{
    public interface IMovieEpisodeRepository
    {
        void SaveMovieEpisode(MovieEpisode movieepisode);
        MovieEpisode GetMovieEpisodeById(int id);
        void DeleteMovieEpisode(MovieEpisode movieepisode);
        void UpdateMovieEpisode(MovieEpisode movieepisode);
        List<MovieEpisode> GetMovieEpisodes();
        MovieEpisode GetMovieSourceById(int id);
        void AddMovieView(MovieView movieview);
    }
}
