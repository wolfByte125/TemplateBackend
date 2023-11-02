using Microsoft.Identity.Client;

namespace Backend.DTOs.UserDTOs.UserAccountDTOs
{
    public class UpdateUserAccountDTO
    {
        public string Id { get; set; } = string.Empty;

        public string Username { get; set; } = string.Empty;

        public int UserRoleId { get; set; }

        public string FirstName { get; set; } = null!;

        public string MiddleName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string Gender { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string PhoneNumber { get; set; } = string.Empty;

    }
}
