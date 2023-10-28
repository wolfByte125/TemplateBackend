using Backend.DTOs.UserDTOs.UserAccountDTOs;
using Backend.Services.UserAccountService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;

namespace Backend.Controllers.UserControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserAccountController : ControllerBase
    {
        private readonly IUserAccountService _userAccountService;

        public UserAccountController(IUserAccountService userAccountService)
        {
            _userAccountService = userAccountService;
        }

        #region READS

        [HttpGet("{id}")]
        public async Task<ActionResult> ReadUserAccountById(string id)
        {
            try
            {
                return Ok(await _userAccountService.ReadUserAccountById(id: id));
            }
            catch (Exception ex)
            {
                return this.ParseException(ex);
            }
        }

        [HttpGet]
        public async Task<ActionResult> ReadUserAccounts([FromQuery] FilterUserAccountDTO filterDTO)
        {
            try
            {
                return Ok(await _userAccountService.ReadUserAccounts(filterDTO: filterDTO));
            }
            catch (Exception ex)
            {
                return this.ParseException(ex);
            }
        }

        #endregion

        #region MANAGE ACCOUNT
        
        [HttpPut("update"), Authorize(Policy = AUTHORIZATION.EXCLUDE_INACTIVE)]
        public async Task<ActionResult> UpdateUserAccount(UpdateUserAccountDTO updateDTO)
        {
            try
            {
                return Ok(await _userAccountService.UpdateUserAccount(updateDTO: updateDTO));
            }
            catch (Exception ex)
            {
                return this.ParseException(ex);
            }
        }

        [HttpDelete("delete"), Authorize(Policy = AUTHORIZATION.EXCLUDE_INACTIVE)]
        public async Task<ActionResult> DeleteUserAccount(string id)
        {
            try
            {
                return Ok(await _userAccountService.DeleteUserAccount(id: id));
            }
            catch (Exception ex)
            {
                return this.ParseException(ex);
            }
        }

        [HttpPut("activate"), Authorize(Policy = AUTHORIZATION.EXCLUDE_INACTIVE)]
        public async Task<ActionResult> ActivateUserAccount(string id)
        {
            try
            {
                return Ok(await _userAccountService.ActivateUserAccount(id: id));
            }
            catch (Exception ex)
            {
                return this.ParseException(ex);
            }
        }

        [HttpPut("deactivate"), Authorize(Policy = AUTHORIZATION.EXCLUDE_INACTIVE)]
        public async Task<ActionResult> DeactivateUserAccount(string id)
        {
            try
            {
                return Ok(await _userAccountService.DeactivateUserAccount(id: id));
            }
            catch (Exception ex)
            {
                return this.ParseException(ex);
            }
        }

        [HttpPut("ban"), Authorize(Policy = AUTHORIZATION.EXCLUDE_INACTIVE)]
        public async Task<ActionResult> BanUserAccount(BanUserAccountDTO banDTO)
        {
            try
            {
                return Ok(await _userAccountService.BanUserAccount(banDTO: banDTO));
            }
            catch (Exception ex)
            {
                return this.ParseException(ex);
            }
        }

        #endregion
    }
}
