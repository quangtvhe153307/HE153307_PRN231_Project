using APIProject.DTO.MovieRated;
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
    public class MovieRatedsController : ODataController
    {
        private IMovieRatedRepository repository = new MovieRatedRepository();
        private readonly IMapper _mapper;

        public MovieRatedsController(IMapper mapper)
        {
            _mapper = mapper;
        }
        [EnableQuery(PageSize = 10)]
        public ActionResult<IQueryable<GetMovieRatedResponseDTO>> Get()
        {
            List<MovieRated> movierateds = repository.GetMovieRateds();
            List<GetMovieRatedResponseDTO> getMovieratedResponseDTOs = _mapper.Map<List<GetMovieRatedResponseDTO>>(movierateds);
            return Ok(getMovieratedResponseDTOs);
        }
        [EnableQuery]
        public ActionResult<double> Get([FromRoute] int key)
        {
            double movierated = repository.GetMovieAverageRatedById(key);
            return Ok(movierated);
        }
        [EnableQuery]
        public IActionResult Post([FromBody] CreateMovieRatedRequestDTO createMovieratedRequestDTO)
        {
            MovieRated movierated = _mapper.Map<MovieRated>(createMovieratedRequestDTO);
            movierated.UserId = LoggedUserId();
            movierated.RatedTime = DateTime.Now;
            repository.SaveMovieRated(movierated);
            GetMovieRatedResponseDTO responseDTO = _mapper.Map<GetMovieRatedResponseDTO>(movierated);
            return Created(responseDTO);
        }
        [EnableQuery]
        public ActionResult Put([FromRoute] int key, [FromBody] UpdateMovieRatedRequestDTO updateMovieratedRequestDTO)
        {
            if (key != updateMovieratedRequestDTO.MovieId)
            {
                return BadRequest();
            }
            MovieRated tempMovierated = repository.GetMovieRated(updateMovieratedRequestDTO.MovieId, updateMovieratedRequestDTO.UserId);
            if (tempMovierated == null)
            {
                return NotFound();
            }
            MovieRated movierated = _mapper.Map<MovieRated>(updateMovieratedRequestDTO);
            movierated.RatedTime = DateTime.Now;
            repository.UpdateMovieRated(movierated);
            return Updated(movierated);
        }
        //[EnableQuery]
        //public ActionResult Delete([FromRoute] int key)
        //{
        //    Movierated tempMovierated = repository.GetMovieratedById(key);
        //    if (tempMovierated == null)
        //    {
        //        return NotFound();
        //    }
        //    repository.DeleteMovierated(tempMovierated);
        //    return NoContent();
        //}
        private int LoggedUserId()
        {
            var userIdString = User.Claims.ToList()[4].Value;
            int userId = Int32.Parse(userIdString);
            return userId;
        }
    }
}
