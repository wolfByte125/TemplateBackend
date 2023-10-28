namespace Backend.DTOs.UserDTOs.UserAccountDTOs
{
    public class RegisterUserAccountDTO
    {
        public string FirstName { get; set; } = null!;

        public string MiddleName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string PhoneNumber { get; set; } = string.Empty;

        public string Username { get; set; } = null!;

        public string Password { get; set; } = null!;
    }
}
