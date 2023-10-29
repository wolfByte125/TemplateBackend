using Backend.DTOs.UserDTOs.UserRoleDTOs;
using Backend.Models.UserModels;

namespace Backend.Services.UserRoleServices
{
    public interface IUserRoleService
    {
        Task<UserRole> CreateUserRole(CreateUserRoleDTO userRoleDTO);
        Task<UserRole> DeleteUserRole(int id);
        Task<UserRole> ReadUserRoleById(int id);
        Task<List<UserRole>> ReadUserRoles(FilterUserRoleDTO filterDTO);
        Task<UserRole> UpdateUserRole(UserRole userRoleDTO);
    }
}
