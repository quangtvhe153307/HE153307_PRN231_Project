using APIProject.DTO.Movie;
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
    [Authorize(Roles = "Administrator")]
    public class MoviesController : ODataController
    {
        private IMovieRepository repository;
        private readonly IMapper _mapper;

        public MoviesController(IMapper mapper, IMovieRepository movieRepository)
        {
            _mapper = mapper;
            repository= movieRepository;
        }
        [EnableQuery(PageSize = 10)]
        public ActionResult<IQueryable<GetMovieResponseDTO>> Get()
        {
            List<Movie> movies = repository.GetMovies();
            List<GetMovieResponseDTO> getMovieResponseDTOs = _mapper.Map<List<GetMovieResponseDTO>>(movies);
            return Ok(getMovieResponseDTOs);
        }
        [EnableQuery]
        public ActionResult<GetMovieResponseDTO> Get([FromRoute] int key)
        {
            Movie movie = repository.GetMovieById(key);
            if (movie == null)
            {
                return NotFound();
            }
            GetMovieResponseDTO getMovieResponseDTO = _mapper.Map<GetMovieResponseDTO>(movie);
            return Ok(getMovieResponseDTO);
        }
        [EnableQuery]
        public IActionResult Post([FromBody] CreateMovieRequestDTO createMovieRequestDTO)
        {
            Movie movie = _mapper.Map<Movie>(createMovieRequestDTO);
            repository.SaveMovie(movie);

            GetMovieResponseDTO responseDTO = _mapper.Map<GetMovieResponseDTO>(movie);
            return Created(responseDTO);
        }
        [EnableQuery]
        public ActionResult Put([FromRoute] int key, [FromBody] UpdateMovieRequestDTO updateMovieRequestDTO)
        {
            if (key != updateMovieRequestDTO.MovieId)
            {
                return BadRequest();
            }
            Movie tempMovie = repository.GetMovieById(key);
            if (tempMovie == null)
            {
                return NotFound();
            }
            Movie movie = _mapper.Map<Movie>(updateMovieRequestDTO);

            repository.UpdateMovie(movie);
            return Updated(movie);
        }
        [EnableQuery]
        public ActionResult Delete([FromRoute] int key)
        {
            Movie tempMovie = repository.GetMovieById(key);
            if (tempMovie == null)
            {
                return NotFound();
            }
            repository.DeleteMovie(tempMovie);
            return NoContent();
        }
    }
}
