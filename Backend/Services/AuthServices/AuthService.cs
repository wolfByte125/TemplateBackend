using Backend.DTOs.AuthDTOs.LoginDTOs;
using Backend.DTOs.AuthDTOs.PasswordDTOs;
using Backend.Models.UserModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;

namespace Backend.Services.AuthServices
{
    public class AuthService : IAuthService
    {
        private const int TOKEN_EXPIRE_HOURS = 15;
        private readonly DataContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;

        public UserAccount UserAccount { get; }
        public UserRole UserRole { get; set; }
        public Permissions Permissions { get; set; }

        public AuthService(DataContext context, IHttpContextAccessor httpContextAccessor, 
            IConfiguration configuration)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
            UserAccount = context.UserAccounts
                .Include(x => x.UserRole.Permissions)
                .Where(u => u.Username == GetMyName())
                .FirstOrDefault();

            UserRole = UserAccount?.UserRole;
            Permissions = UserAccount?.UserRole?.Permissions;
        }

        public string GetMyName()
        {
            var result = string.Empty;

            if (_httpContextAccessor.HttpContext != null)
            {
                result = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Name);
            }

            return result;
        }

        // LOGIN
        public LoginReturnDTO Login(LoginDTO loginDTO)
        {
            UserAccount? userAccount = _context.UserAccounts
                .FirstOrDefault(x => x.Username == loginDTO.Username)
                ??
                throw new KeyNotFoundException("Invalid Username or Password.");

            if (!VerifyPasswordHash(loginDTO.Password, userAccount.PasswordHash, userAccount.PasswordSalt))
            {
                throw new InvalidOperationException("Invalid Username or Password.");
            }

            if (userAccount.Status == USER_STATUS.INACTIVE)
                throw new InvalidOperationException("User Account Has Been Deactivated.");

            string token = CreateToken(userAccount);

            var returnDTO = new LoginReturnDTO()
            {
                UserAccountId = userAccount.Id,
                ExpiresOn = DateTime.Now.AddHours(TOKEN_EXPIRE_HOURS),
                Token = token,
                Username = userAccount.Username,
            };

            return returnDTO;
        }

        #region MANAGE PASSWORD
        
        // CHANGE PASSWORD
        public bool ChangePassword(ChangePasswordDTO changePasswordDTO)
        {
            LoginDTO loginDTO = new();

            loginDTO.Username = changePasswordDTO.Username;
            loginDTO.Password = changePasswordDTO.Password;

            Login(loginDTO);

            if (changePasswordDTO.NewPassword.Length < 8)
                throw new InvalidOperationException("Password Is Too Short!");

            UserAccount userAccount = _context.UserAccounts.First(u => u.Username == changePasswordDTO.Username);

            CreatePasswordHash(changePasswordDTO.NewPassword, out byte[] passwordHash, out byte[] passwordSalt);

            userAccount.PasswordHash = passwordHash;
            userAccount.PasswordSalt = passwordSalt;

            _context.UserAccounts.Update(userAccount);
            _context.SaveChanges();

            return true;
        }

        // RESET PASSWORD
        public bool ResetPassword(ResetPasswordDTO resetPasswordDTO)
        {
            UserAccount? userAccount = _context.UserAccounts
                .FirstOrDefault(u => u.Username == resetPasswordDTO.Username)
                ??
                throw new InvalidOperationException("Username Not Found!");

            if (resetPasswordDTO.NewPassword.Length < 8)
                throw new InvalidOperationException("Password Is Too Short!");

            CreatePasswordHash(resetPasswordDTO.NewPassword, out byte[] passwordHash, out byte[] passwordSalt);

            userAccount.PasswordHash = passwordHash;
            userAccount.PasswordSalt = passwordSalt;

            _context.UserAccounts.Update(userAccount);
            _context.SaveChanges();

            return true;
        }

        #endregion

        #region TOKEN

        private string CreateToken(UserAccount userAccount)
        {

            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, userAccount.Username.ToString()),
                new Claim(ClaimTypes.Rsa, userAccount.UserRoleId.ToString()),
                new Claim(ClaimTypes.Sid, userAccount.Id.ToString()),
                new Claim(ClaimTypes.Role, AssignRole(userAccount))
            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
                _configuration.GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddHours(TOKEN_EXPIRE_HOURS),
                signingCredentials: creds);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }

        private string AssignRole(UserAccount userAccount)
        {
            return userAccount.Status == USER_STATUS.ACTIVE ? userAccount.UserRole.RoleName : AUTHORIZATION.UNAUTHORIZED_ACCOUNT;
        }

        #endregion

        #region PASSWORD HASH - CREATE | VERFY

        public void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }

        #endregion

    }
}
