using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace Backend.DTOs.UserDTOs.UserAccountDTOs
{
    public class BanUserAccountDTO
    {
        public string UserAccountId { get; set; } = null!;

        public string ReasonForBan { get; set; } = null!;
    }
}
