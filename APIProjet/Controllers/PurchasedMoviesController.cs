using APIProject.DTO.PurchasedMovie;
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
    public class PurchasedMoviesController : ODataController
    {
        private IPurchasedMovieRepository repository;
        private readonly IMapper _mapper;

        public PurchasedMoviesController(IMapper mapper, IPurchasedMovieRepository purchasedMovieRepository)
        {
            _mapper = mapper;
            repository= purchasedMovieRepository;
        }
        [EnableQuery(PageSize = 10)]
        public ActionResult<IQueryable<GetPurchasedMovieResponseDTO>> Get()
        {
            List<PurchasedMovie> purchasedmovies = repository.GetPurchasedMovies();
            List<GetPurchasedMovieResponseDTO> getPurchasedmovieResponseDTOs = _mapper.Map<List<GetPurchasedMovieResponseDTO>>(purchasedmovies);
            return Ok(getPurchasedmovieResponseDTOs);
        }
        [EnableQuery]
        public ActionResult<IQueryable<GetPurchasedMovieResponseDTO>> Get([FromRoute] int key)
        {
            List<PurchasedMovie> purchasedmovies = repository.GetPurchasedMoviesById(key);
            List<GetPurchasedMovieResponseDTO> getPurchasedmovieResponseDTO = _mapper.Map<List<GetPurchasedMovieResponseDTO>>(purchasedmovies);
            return Ok(getPurchasedmovieResponseDTO);
        }
        [HttpGet("/MyPurchase")]
        public ActionResult<IQueryable<GetPurchasedMovieResponseDTO>> GetMyPurchase()
        {
            try
            {
                int key = LoggedUserId();
                List<PurchasedMovie> purchasedmovies = repository.GetPurchasedMoviesById(key);
                List<GetPurchasedMovieResponseDTO> getPurchasedmovieResponseDTO = _mapper.Map<List<GetPurchasedMovieResponseDTO>>(purchasedmovies);
                return Ok(getPurchasedmovieResponseDTO);
            } catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [EnableQuery]
        public IActionResult Post([FromBody] CreatePurchasedMovieRequestDTO createPurchasedMovieRequestDTO)
        {
            try
            {
                PurchasedMovie purchasedMovie = _mapper.Map<PurchasedMovie>(createPurchasedMovieRequestDTO);
                purchasedMovie.PurchasedTime = DateTime.Now;
                purchasedMovie.UserId = LoggedUserId();
                repository.SavePurchasedMovies(purchasedMovie);

                GetPurchasedMovieResponseDTO responseDTO = _mapper.Map<GetPurchasedMovieResponseDTO>(purchasedMovie);
                return Created(responseDTO);
            } catch (Exception ex)
            {
                return BadRequest(new { message = "error" });
            }
        }
        //[EnableQuery]
        //public ActionResult Put([FromRoute] int key, [FromBody] UpdatePurchasedMovieRequestDTO updatePurchasedMovieRequestDTO)
        //{
        //    if (key != updatePurchasedMovieRequestDTO.UserId)
        //    {
        //        return BadRequest();
        //    }
        //    Purchasedmovie tempPurchasedmovie = repository.GetPurchasedmovieById(key);
        //    if (tempPurchasedmovie == null)
        //    {
        //        return NotFound();
        //    }
        //    Purchasedmovie purchasedmovie = _mapper.Map<Purchasedmovie>(updatePurchasedmovieRequestDTO);
        //    purchasedmovie.PurchasedmovieId = tempPurchasedmovie.PurchasedmovieId;
        //    repository.UpdatePurchasedmovie(purchasedmovie);
        //    return Updated(purchasedmovie);
        //}
        //[EnableQuery]
        //public ActionResult Delete([FromRoute] int key)
        //{
        //    Purchasedmovie tempPurchasedmovie = repository.GetPurchasedmovieById(key);
        //    if (tempPurchasedmovie == null)
        //    {
        //        return NotFound();
        //    }
        //    repository.DeletePurchasedmovie(tempPurchasedmovie);
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
