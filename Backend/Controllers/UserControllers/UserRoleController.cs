using Backend.DTOs.UserDTOs.UserRoleDTOs;
using Backend.Models.UserModels;
using Backend.Services.UserRoleServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers.UserControllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = AUTHORIZATION.EXCLUDE_INACTIVE)]
    public class UserRoleController : ControllerBase
    {
        private readonly IUserRoleService _userRoleService;

        public UserRoleController(IUserRoleService userRoleService)
        {
            _userRoleService = userRoleService;
        }

        [HttpPost]
        public async Task<ActionResult> CreateUserRole(CreateUserRoleDTO userRoleDTO)
        {
            try
            {
                return Ok(await _userRoleService.CreateUserRole(userRoleDTO: userRoleDTO));
            }
            catch (Exception ex)
            {
                return this.ParseException(ex);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> ReadUserRoleById(int id)
        {
            try
            {
                return Ok(await _userRoleService.ReadUserRoleById(id: id));
            }
            catch (Exception ex)
            {
                return this.ParseException(ex);
            }
        }

        [HttpGet]
        public async Task<ActionResult> ReadUserRoles([FromQuery] FilterUserRoleDTO filterDTO)
        {
            try
            {
                return Ok(await _userRoleService.ReadUserRoles(filterDTO: filterDTO));
            }
            catch (Exception ex)
            {
                return this.ParseException(ex);
            }
        }

        [HttpPut]
        public async Task<ActionResult> UpdateUserRole(UserRole userRoleDTO)
        {
            try
            {
                return Ok(await _userRoleService.UpdateUserRole(userRoleDTO: userRoleDTO));
            }
            catch (Exception ex)
            {
                return this.ParseException(ex);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteUserRole(int id)
        {
            try
            {
                return Ok(await _userRoleService.DeleteUserRole(id: id));
            }
            catch (Exception ex)
            {
                return this.ParseException(ex);
            }
        }
    }
}
