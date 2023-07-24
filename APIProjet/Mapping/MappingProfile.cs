using APIProject.DTO;
using APIProject.DTO.Category;
using APIProject.DTO.CategoryMovie;
using APIProject.DTO.Comment;
using APIProject.DTO.Movie;
using APIProject.DTO.MovieEpisode;
using APIProject.DTO.MovieRated;
using APIProject.DTO.MovieSeason;
using APIProject.DTO.PurchasedMovie;
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
            CreateMap<Transaction, GetTransactionResponseDTO>()
                .ForMember(dest => dest.TransactionDateStr,
                options => options.MapFrom(source => source.TransactionDate.ToShortDateString()));
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
            CreateMap<GetMovieResponseDTO, GetMovieByRankResponseDTO>();
            CreateMap<Movie, GetMovieResponseDTO>()
                .ForMember(destination => destination.ViewCount,
                options => options.MapFrom(source => source.MovieSeasons.Sum(x => x.MovieEpisodes.Sum(me => me.MovieViews == null ? 0 : me.MovieViews.Count))));
                //options => options.MapFrom(source => source.MovieEpisodes.Sum(x => x.MovieViews.Count)));

            CreateMap<CreateMovieSeasonRequestDTO, MovieSeason>();
            CreateMap<UpdateMovieSeasonRequestDTO, MovieSeason>();
            CreateMap<MovieSeason, GetMovieSeasonResponseDTO>()
                .ForMember(destination => destination.ViewCount,
                options => options.MapFrom(source => source.MovieEpisodes.Sum(x => x.MovieViews.Count)));

            CreateMap<CreateMovieEpisodeRequestDTO, MovieEpisode>();
            CreateMap<UpdateMovieEpisodeRequestDTO, MovieEpisode>();
            CreateMap<MovieEpisode, GetMovieEpisodeResponseDTO>()
                .ForMember(destination => destination.ViewCount,
                options => options.MapFrom(source => source.MovieViews.Count));

            //Comment
            CreateMap<TimeSpan, string>().ConvertUsing(typeof(TimeSpanFormatterConverter));
            CreateMap<CreateCommentRequestDTO, Comment>();
            CreateMap<UpdateCommentRequestDTO, Comment>();
            CreateMap<Comment, GetCommentResponseDTO>()
                .ForMember(destination => destination.CommentedTimeInterval,
                options => options.MapFrom<TimeSpanFormatterConverter>())
                .ForMember(destination => destination.CommentedDateStr,
                options => options.MapFrom(source => source.CommentedDate.ToShortDateString()));

            //MovieRate
            CreateMap<CreateMovieRatedRequestDTO, MovieRated>();
            CreateMap<UpdateMovieRatedRequestDTO, MovieRated>();
            CreateMap<MovieRated, GetMovieRatedResponseDTO>();
            
            //PurchasedMovie
            CreateMap<CreatePurchasedMovieRequestDTO, PurchasedMovie>();
            CreateMap<UpdatePurchasedMovieRequestDTO, PurchasedMovie>();
            CreateMap<PurchasedMovie, GetPurchasedMovieResponseDTO>()
                .ForMember(dest => dest.PurchasedTimeStr,
                options => options.MapFrom(source => (source.PurchasedTime != null ? source.PurchasedTime.Value.ToShortDateString() : "")));
        }
    }
}
