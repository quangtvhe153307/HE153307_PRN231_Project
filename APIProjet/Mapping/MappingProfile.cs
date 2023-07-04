using APIProject.DTO;
using APIProject.DTO.Role;
using APIProject.DTO.Transaction;
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
        }
    }
}
