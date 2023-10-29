using AutoMapper;
using Backend.Contexts;
using Backend.DTOs.UserDTOs.UserAccountDTOs;
using Backend.DTOs.UserDTOs.UserRoleDTOs;
using Backend.Models.UserModels;
using Microsoft.AspNetCore.Mvc;
using System.Security;

namespace Backend.Utils
{
    public class AutoMapperProfile : Profile
    {
        private readonly IMapper _mapper;

        public AutoMapperProfile(IMapper mapper, DataContext context)
        {
            _mapper = mapper;
        }
        public AutoMapperProfile()
        {
            #region USER RELATED

            // USER ACCOUNT
            CreateMap<RegisterUserAccountDTO, UserAccount>();
            CreateMap<UpdateUserAccountDTO, UserAccount>();

            // USER ROLE
            CreateMap<CreateUserRoleDTO, UserRole>();
            CreateMap<UserRole, UserRole>();
            CreateMap<Permissions, Permissions>();

            // BAN USER ACCOUNT
            CreateMap<BanUserAccountDTO, BannedAccount>();

            #endregion
        }
    }
}
