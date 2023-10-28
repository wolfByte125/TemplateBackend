namespace Backend.DTOs.AuthDTOs.LoginDTOs
{
    public class LoginReturnDTO
    {
        public string Token { get; set; }
        public string Username { get; set; }
        public DateTime ExpiresOn { get; set; }
        public string? Id { get; set; }
        public string UserAccountId { get; set; }
        public dynamic User { get; set; }
    }
}
