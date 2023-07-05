﻿using APIProject.DTO;
using APIProject.DTO.Category;
using APIProject.DTO.CategoryMovie;
using APIProject.DTO.Movie;
using APIProject.DTO.MovieEpisode;
using APIProject.DTO.MovieSeason;
using APIProject.DTO.Role;
using APIProject.DTO.Transaction;
using APIProject.DTO.User;
using AutoMapper;
using BusinessObjects;
using System.Data;

namespace APIProject.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, AuthenticateResponse>();

            //Role
            CreateMap<Role, GetRoleResponseDTO>();
            CreateMap<CreateRoleRequestDTO, Role>();
            CreateMap<UpdateRoleRequestDTO, Role>();
            
            //Transaction
            CreateMap<Transaction, GetTransactionResponseDTO>();
            CreateMap<CreateTransactionRequestDTO, Transaction>();
            CreateMap<UpdateTransactionRequestDTO, Transaction>();
            
            //Category
            CreateMap<Category, GetCategoryResponseDTO>();
            CreateMap<CreateCategoryRequestDTO, Category>();
            CreateMap<UpdateCategoryRequestDTO, Category>();
            CreateMap<CreateCategoryMovieRequestDTO, Category>();
            
            //User
            CreateMap<User, GetUserResponseDTO>();
            CreateMap<CreateUserRequestDTO, User>();
            CreateMap<UpdateUserRequestDTO, User>();

            //Movie
            CreateMap<CreateMovieRequestDTO, Movie>();
            CreateMap<UpdateMovieRequestDTO, Movie>();
            CreateMap<Movie, GetMovieResponseDTO>();
            CreateMap<CreateMovieSeasonRequestDTO, MovieSeason>();
            CreateMap<MovieSeason, GetMovieSeasonResponseDTO>();
            CreateMap<CreateMovieEpisodeRequestDTO, MovieEpisode>();
            CreateMap<MovieEpisode, GetMovieEpisodeResponseDTO>();
        }
    }
}
