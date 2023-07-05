using APIProject.DTO;
using APIProject.DTO.Category;
using APIProject.DTO.CategoryMovie;
using APIProject.DTO.Comment;
using APIProject.DTO.Movie;
using APIProject.DTO.MovieEpisode;
using APIProject.DTO.MovieRated;
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
            CreateMap<UpdateMovieSeasonRequestDTO, MovieSeason>();
            CreateMap<MovieSeason, GetMovieSeasonResponseDTO>();
            CreateMap<CreateMovieEpisodeRequestDTO, MovieEpisode>();
            CreateMap<UpdateMovieEpisodeRequestDTO, MovieEpisode>();
            CreateMap<MovieEpisode, GetMovieEpisodeResponseDTO>();

            //Comment
            CreateMap<CreateCommentRequestDTO, Comment>();
            CreateMap<UpdateCommentRequestDTO, Comment>();
            CreateMap<Comment, GetCommentResponseDTO>();
            
            //MovieRate
            CreateMap<CreateMovieRatedRequestDTO, MovieRated>();
            CreateMap<UpdateMovieRatedRequestDTO, MovieRated>();
            CreateMap<MovieRated, GetMovieRatedResponseDTO>();
        }
    }
}
