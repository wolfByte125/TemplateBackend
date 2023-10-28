global using Backend.Contexts;
using Backend.DTOs.UserDTOs.UserAccountDTOs;
using Backend.Models.UserModels;
using Backend.Services.AuthServices;
using System.Reflection;

namespace Backend.Services._SeederService
{
    public class SeederService
    {
        private readonly DataContext _context;
        private readonly IAuthService _authService;

        public SeederService(DataContext context, IAuthService authService)
        {
            _context = context;
            _authService = authService;
        }

        public bool SeedDB()
        {
            UserRoleSeeder();
            UserAccountSeeder();

            return true;
        }

        #region USER RELATED

        public void UserRoleSeeder()
        {
            if (_context.UserAccounts.Any() && _context.UserRoles.Any())
            {
                return;
            }

            List<UserRole> userRoles = new();
            List<string> rolePermissions = new();
            rolePermissions.Add("Id");
            rolePermissions.Add("UserRole");
            rolePermissions.Add("UserRoleId");

            userRoles.Add(new UserRole()
            {
                RoleName = SEEDED_ROLES.SUPER_ADMIN,
                IsSuperAdmin = true,
                Permissions = new(),
            });

            foreach (PropertyInfo prop in userRoles[0].Permissions.GetType().GetProperties())
            {
                _ = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;

                if (!rolePermissions.Contains(prop.Name))
                    prop.SetValue(userRoles[0].Permissions, true);
            }

            userRoles.Add(new UserRole()
            {
                RoleName = SEEDED_ROLES.DEFAULT_ROLE,
                IsSuperAdmin = false,
                Permissions = new(),
            });

            _context.UserRoles.AddRange(userRoles);
            _context.SaveChanges();

            Console.WriteLine("User Role Seeded");
        }

        public void UserAccountSeeder()
        {
            if (_context.UserAccounts.Any())
            {
                return;
            }

            _authService.RegisterUserAccount(new RegisterUserAccountDTO()
            {
                FirstName = "Super Admin",
                Username = "SuperAdmin",
                Password = "password",
                IsSuperAdmin = true
            }).Wait();

            Console.WriteLine("Super Admin Account Seeded");
        }

        #endregion
    }
}
