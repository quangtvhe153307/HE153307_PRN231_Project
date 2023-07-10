using APIProject.DTO.Movie;
using APIProject.Util;
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
    public class MoviesController : ODataController
    {
        private IMovieRepository repository;
        private readonly IMapper _mapper;

        public MoviesController(IMapper mapper, IMovieRepository movieRepository)
        {
            _mapper = mapper;
            repository= movieRepository;
        }
        [Authorize(Roles = "Administrator")]
        [EnableQuery(PageSize = 10)]
        public ActionResult<IQueryable<GetMovieResponseDTO>> Get()
        {
            List<Movie> movies = repository.GetMovies();
            List<GetMovieResponseDTO> getMovieResponseDTOs = _mapper.Map<List<GetMovieResponseDTO>>(movies);
            return Ok(getMovieResponseDTOs);
        }
        [HttpGet("/MovieRanking/{rankType}")]
        [EnableQuery]
        public ActionResult<IQueryable<GetMovieResponseDTO>> GetMovieByRanking(int rankType)
        {
            DateTime startDate = DateTimeUtils.GetStartDateRanking(rankType);
            DateTime endDate = DateTimeUtils.GetEndDateRanking();
            List<Movie> movies = repository.GetMoviesByRank(startDate, endDate);
            List<GetMovieResponseDTO> getMovieResponseDTOs = _mapper.Map<List<GetMovieResponseDTO>>(movies);
            getMovieResponseDTOs = getMovieResponseDTOs.OrderByDescending(x => x.ViewCount).Take(10).ToList();
            List<GetMovieByRankResponseDTO> responseDTOs = _mapper.Map<List<GetMovieByRankResponseDTO>>(getMovieResponseDTOs);
            return Ok(responseDTOs);
        }
        [Authorize(Roles = "Administrator")]
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
        [Authorize(Roles = "Administrator")]
        [EnableQuery]
        public IActionResult Post([FromBody] CreateMovieRequestDTO createMovieRequestDTO)
        {
            Movie movie = _mapper.Map<Movie>(createMovieRequestDTO);
            repository.SaveMovie(movie);

            GetMovieResponseDTO responseDTO = _mapper.Map<GetMovieResponseDTO>(movie);
            return Created(responseDTO);
        }
        [Authorize(Roles = "Administrator")]
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
        [Authorize(Roles = "Administrator")]
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
