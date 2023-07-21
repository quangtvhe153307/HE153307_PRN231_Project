using APIProject.DTO.MovieEpisode;
using AutoMapper;
using BusinessObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Repository.IRepository;
using Repository.Repository;
using System.Data;

namespace APIProject.Controllers
{
    [Authorize(Roles = "Administrator,VIP,Normal")]
    public class MovieEpisodesController : ODataController
    {
        private IMovieEpisodeRepository repository;
        private IMovieRepository movieRepository;
        
        private readonly IMapper _mapper;

        public MovieEpisodesController(IMapper mapper, IMovieEpisodeRepository movieEpisodeRepository, IMovieRepository movieRepository)
        {
            _mapper = mapper;
            repository= movieEpisodeRepository;
            this.movieRepository = movieRepository;
        }
        [Authorize(Roles = "Administrator")]
        [EnableQuery(PageSize = 10)]
        public ActionResult<IQueryable<GetMovieEpisodeResponseDTO>> Get()
        {
            List<MovieEpisode> movieepisodes = repository.GetMovieEpisodes();
            List<GetMovieEpisodeResponseDTO> getMovieepisodeResponseDTOs = _mapper.Map<List<GetMovieEpisodeResponseDTO>>(movieepisodes);
            return Ok(getMovieepisodeResponseDTOs);
        }
        [Authorize(Roles = "Administrator")]
        [EnableQuery]
        public ActionResult<GetMovieEpisodeResponseDTO> Get([FromRoute] int key)
        {
            MovieEpisode movieepisode = repository.GetMovieEpisodeById(key);
            if (movieepisode == null)
            {
                return NotFound();
            }
            GetMovieEpisodeResponseDTO getMovieepisodeResponseDTO = _mapper.Map<GetMovieEpisodeResponseDTO>(movieepisode);
            return Ok(getMovieepisodeResponseDTO);
        }
        [Authorize(Roles = "Administrator,VIP,Normal")]
        [HttpGet("/MovieSource/{key}")]
        public ActionResult<string> GetMovieUrl([FromRoute] int key)
        {
            string role = User.Claims.ToList()[3].Value;
            MovieEpisode movieepisode = repository.GetMovieSourceById(key);

            bool isFree = movieRepository.CheckFreeMovie(movieepisode.MovieSeason.MovieId);
            if (!isFree)
            {
                if (role.Equals("Normal"))
                {

                    bool isPurchased = movieRepository.IsPurchased(LoggedUserId(), movieepisode.MovieSeason.MovieId);
                    Console.WriteLine(isPurchased);
                    if (!isPurchased)
                    {
                        return Forbid();
                    }
                }
            }
            if (movieepisode == null)
            {
                return NotFound();
            }
            repository.AddMovieView(new MovieView
            {
                EpisodeId = key,
                UserId = LoggedUserId(),
                ViewedDate = DateTime.Now
            });
            repository.UpdateMovieEpisode(movieepisode);
            return Ok(movieepisode.UrlSource);
        }
        [Authorize(Roles = "Administrator")]
        [EnableQuery]
        public IActionResult Post([FromBody] CreateMovieEpisodeRequestDTO createMovieepisodeRequestDTO)
        {
            MovieEpisode movieepisode = _mapper.Map<MovieEpisode>(createMovieepisodeRequestDTO);
            repository.SaveMovieEpisode(movieepisode);

            GetMovieEpisodeResponseDTO responseDTO = _mapper.Map<GetMovieEpisodeResponseDTO>(movieepisode);
            return Created(responseDTO);
        }
        [Authorize(Roles = "Administrator")]
        [EnableQuery]
        public ActionResult Put([FromRoute] int key, [FromBody] UpdateMovieEpisodeRequestDTO updateMovieepisodeRequestDTO)
        {
            if (key != updateMovieepisodeRequestDTO.EpisodeId)
            {
                return BadRequest();
            }
            MovieEpisode tempMovieepisode = repository.GetMovieEpisodeById(key);
            if (tempMovieepisode == null)
            {
                return NotFound();
            }
            MovieEpisode movieepisode = _mapper.Map<MovieEpisode>(updateMovieepisodeRequestDTO);
            movieepisode.EpisodeId = tempMovieepisode.EpisodeId;
            repository.UpdateMovieEpisode(movieepisode);
            return Updated(movieepisode);
        }
        [Authorize(Roles = "Administrator")]
        [EnableQuery]
        public ActionResult Delete([FromRoute] int key)
        {
            MovieEpisode tempMovieepisode = repository.GetMovieEpisodeById(key);
            if (tempMovieepisode == null)
            {
                return NotFound();
            }
            repository.DeleteMovieEpisode(tempMovieepisode);
            return NoContent();
        }
        private int LoggedUserId()
        {
            var userIdString = User.Claims.ToList()[4].Value;
            int userId = Int32.Parse(userIdString);
            return userId;
        }
    }
}
