using Backend.DTOs.AuthDTOs.LoginDTOs;
using Backend.DTOs.AuthDTOs.PasswordDTOs;
using Backend.Models.UserModels;

namespace Backend.Services.AuthServices
{
    public interface IAuthService
    {
        Permissions Permissions { get; set; }
        UserAccount UserAccount { get; }
        UserRole UserRole { get; set; }

        bool ChangePassword(ChangePasswordDTO changePasswordDTO);
        void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt);
        string GetMyName();
        LoginReturnDTO Login(LoginDTO loginDTO);
        bool ResetPassword(ResetPasswordDTO resetPasswordDTO);
    }
}
