using Backend.DTOs.AuthDTOs.LoginDTOs;
using Backend.DTOs.AuthDTOs.PasswordDTOs;
using Backend.DTOs.UserDTOs.UserAccountDTOs;
using Backend.Services.AuthServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers.UserControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpGet("check/token"), Authorize]
        public ActionResult<string> GetTokenValid()
        {
            var userName = _authService.GetMyName();
            return Ok(new { Status = true, UserName = userName });
        }

        [HttpGet("check/account"), Authorize(Policy = AUTHORIZATION.EXCLUDE_INACTIVE)]
        public ActionResult<string> GetAccountValid()
        {
            var userName = _authService.GetMyName();
            return Ok(new { Status = true, UserName = userName });
        }


        [HttpPost("register")]
        public async Task<ActionResult> RegisterUserAccount(RegisterUserAccountDTO registerDTO)
        {
            try
            {
                return Ok(await _authService.RegisterUserAccount(registerDTO));
            }
            catch (Exception ex)
            {
                return this.ParseException(ex);
            }
        }

        [HttpPost("login")]
        public ActionResult Login(LoginDTO loginDTO)
        {
            try
            {
                return Ok(_authService.Login(loginDTO));
            }
            catch (Exception ex)
            {
                return this.ParseException(ex);
            }
        }

        [HttpPost("changePassword"), Authorize]
        public ActionResult ChangePassword(ChangePasswordDTO changePasswordDTO)
        {
            try
            {
                return Ok(_authService.ChangePassword(changePasswordDTO));
            }
            catch (Exception ex)
            {
                return this.ParseException(ex);
            }
        }

        [HttpPost("resetPassword"), Authorize(Policy = AUTHORIZATION.EXCLUDE_INACTIVE)]
        public ActionResult ResetPassword(ResetPasswordDTO resetPasswordDTO)
        {
            try
            {
                return Ok(_authService.ResetPassword(resetPasswordDTO));
            }
            catch (Exception ex)
            {
                return this.ParseException(ex);
            }
        }
    }
}
