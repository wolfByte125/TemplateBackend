using AutoMapper;
using Backend.DTOs.UserDTOs.UserRoleDTOs;
using Backend.Models.UserModels;
using Backend.Services.AuthServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace Backend.Services.UserRoleServices
{
    public class UserRoleService : IUserRoleService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;

        public UserRoleService(DataContext context, IMapper mapper, 
            IAuthService authService)
        {
            _context = context;
            _mapper = mapper;
            _authService = authService;
        }

        // CREATE USER ROLE
        public async Task<UserRole> CreateUserRole(CreateUserRoleDTO userRoleDTO)
        {
            UserRole userRole = _mapper.Map<UserRole>(userRoleDTO);

            userRole.Permissions = new();

            _context.UserRoles.Add(userRole);
            await _context.SaveChangesAsync();

            return userRole;
        }

        // READ BY ID
        public async Task<UserRole> ReadUserRoleById(int id)
        {
            UserRole? userRole = await _context.UserRoles
                .Where(x => x.Id == id)
                .Include(x => x.Permissions)
                .FirstOrDefaultAsync()
                ??
                throw new KeyNotFoundException("User Role Not Found.");

            return userRole;
        }

        // READ ALL
        public async Task<List<UserRole>> ReadUserRoles(FilterUserRoleDTO filterDTO)
        {
            List<UserRole> userRoles = await _context.UserRoles
                .Where(x =>
                    (filterDTO.IsSuperAdmin == null || x.IsSuperAdmin == filterDTO.IsSuperAdmin))
                .ToListAsync();

            return userRoles;
        }

        // UPDATE USER ROLE
        public async Task<UserRole> UpdateUserRole(UserRole userRoleDTO)
        {
            var userRole = await _context.UserRoles
                .AsNoTracking()
                .Where(x => x.Id == userRoleDTO.Id)
                .Include(x => x.Permissions)
                .FirstOrDefaultAsync()
                ??
                throw new KeyNotFoundException("User Role Not Found.");

            userRole.RoleName = userRoleDTO.RoleName;
            userRole.IsSuperAdmin = userRoleDTO.IsSuperAdmin;

            if (userRoleDTO.Permissions != null)
            {
                if (userRole.Permissions != null)
                    userRoleDTO.Permissions.Id = userRole.Permissions.Id;
                    
                userRole.Permissions = userRoleDTO.Permissions;
            }
            
            userRole.Permissions ??= new();

            if (_authService.UserRole.Id == userRoleDTO.Id)
            {
                if (_authService.UserRole.Permissions != null && userRoleDTO.Permissions != null)
                {
                    _authService.Permissions = _mapper.Map(userRoleDTO.Permissions, _authService.UserRole.Permissions);
                    _context.Update(_authService.Permissions);
                }

                _authService.UserRole.IsSuperAdmin = userRoleDTO.IsSuperAdmin;
                await _context.SaveChangesAsync();

            }
            
            return userRole;
        }


        // DELETE USER ROLE
        public async Task<UserRole> DeleteUserRole(int id)
        {
            UserRole userRole = await ReadUserRoleById(id: id);

            try
            {
                _context.Remove(userRole);
                await _context.SaveChangesAsync();
            }
            // IN CASE THERE ARE ANY REFERENCES
            catch (DbUpdateException)
            {
                throw new InvalidOperationException("User Role Can Not Be Deleted.");
            }

            return userRole;
        }
    }
}
