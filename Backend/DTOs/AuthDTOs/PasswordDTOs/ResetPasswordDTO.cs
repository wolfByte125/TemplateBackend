namespace Backend.DTOs.AuthDTOs.PasswordDTOs
{
    public class ResetPasswordDTO
    {
        public string Username { get; set; } = string.Empty;
        
        public string Password { get; set; } = string.Empty;

        public string NewPassword { get; set; } = string.Empty;
    }
}
