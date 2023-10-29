namespace Backend.DTOs.UserDTOs.UserRoleDTOs
{
    public class CreateUserRoleDTO
    {
        public string RoleName { get; set; } = null!;

        public bool IsSuperAdmin { get; set; } = false;
    }
}
