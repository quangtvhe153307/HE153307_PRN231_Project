using APIProject.DTO;
using APIProject.DTO.Category;
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
            
            //User
            CreateMap<User, GetUserResponseDTO>();
            CreateMap<CreateUserRequestDTO, User>();
            CreateMap<UpdateUserRequestDTO, User>();
        }
    }
}
