using APIProject.DTO.MovieEpisode;
using AutoMapper;
using BusinessObjects;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Repository.IRepository;
using Repository.Repository;

namespace APIProject.Controllers
{
    public class MovieEpisodesController : ODataController
    {
        private IMovieEpisodeRepository repository = new MovieEpisodeRepository();
        private readonly IMapper _mapper;

        public MovieEpisodesController(IMapper mapper)
        {
            _mapper = mapper;
        }
        [EnableQuery(PageSize = 10)]
        public ActionResult<IQueryable<GetMovieEpisodeResponseDTO>> Get()
        {
            List<MovieEpisode> movieepisodes = repository.GetMovieEpisodes();
            List<GetMovieEpisodeResponseDTO> getMovieepisodeResponseDTOs = _mapper.Map<List<GetMovieEpisodeResponseDTO>>(movieepisodes);
            return Ok(getMovieepisodeResponseDTOs);
        }
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
        [EnableQuery]
        public IActionResult Post([FromBody] CreateMovieEpisodeRequestDTO createMovieepisodeRequestDTO)
        {
            MovieEpisode movieepisode = _mapper.Map<MovieEpisode>(createMovieepisodeRequestDTO);
            repository.SaveMovieEpisode(movieepisode);

            GetMovieEpisodeResponseDTO responseDTO = _mapper.Map<GetMovieEpisodeResponseDTO>(movieepisode);
            return Created(responseDTO);
        }
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
    }
}
