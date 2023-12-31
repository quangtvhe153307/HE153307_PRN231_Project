﻿using APIProject.DTO.MovieSeason;
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
    public class MovieSeasonsController : ODataController
    {
        private IMovieSeasonRepository repository;
        private readonly IMapper _mapper;

        public MovieSeasonsController(IMapper mapper, IMovieSeasonRepository movieSeasonRepository)
        {
            _mapper = mapper;
            repository= movieSeasonRepository;
        }
        [EnableQuery(PageSize = 10)]
        public ActionResult<IQueryable<GetMovieSeasonResponseDTO>> Get()
        {
            List<MovieSeason> movieseasons = repository.GetMovieSeasons();
            List<GetMovieSeasonResponseDTO> getMovieseasonResponseDTOs = _mapper.Map<List<GetMovieSeasonResponseDTO>>(movieseasons);
            return Ok(getMovieseasonResponseDTOs);
        }
        [EnableQuery]
        public ActionResult<GetMovieSeasonResponseDTO> Get([FromRoute] int key)
        {
            MovieSeason movieseason = repository.GetMovieSeasonById(key);
            if (movieseason == null)
            {
                return NotFound();
            }
            GetMovieSeasonResponseDTO getMovieseasonResponseDTO = _mapper.Map<GetMovieSeasonResponseDTO>(movieseason);
            return Ok(getMovieseasonResponseDTO);
        }
        [EnableQuery]
        public IActionResult Post([FromBody] CreateMovieSeasonRequestDTO createMovieseasonRequestDTO)
        {
            MovieSeason movieseason = _mapper.Map<MovieSeason>(createMovieseasonRequestDTO);
            repository.SaveMovieSeason(movieseason);

            GetMovieSeasonResponseDTO responseDTO = _mapper.Map<GetMovieSeasonResponseDTO>(movieseason);
            return Created(responseDTO);
        }
        [EnableQuery]
        public ActionResult Put([FromRoute] int key, [FromBody] UpdateMovieSeasonRequestDTO updateMovieseasonRequestDTO)
        {
            if (key != updateMovieseasonRequestDTO.MovieSeasonId)
            {
                return BadRequest();
            }
            MovieSeason tempMovieseason = repository.GetMovieSeasonById(key);
            if (tempMovieseason == null)
            {
                return NotFound();
            }
            MovieSeason movieseason = _mapper.Map<MovieSeason>(updateMovieseasonRequestDTO);
            movieseason.MovieSeasonId = tempMovieseason.MovieSeasonId;
            repository.UpdateMovieSeason(movieseason);
            return Updated(movieseason);
        }
        [EnableQuery]
        public ActionResult Delete([FromRoute] int key)
        {
            MovieSeason tempMovieseason = repository.GetMovieSeasonById(key);
            if (tempMovieseason == null)
            {
                return NotFound();
            }
            repository.DeleteMovieSeason(tempMovieseason);
            return NoContent();
        }
    }
}
