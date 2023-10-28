using Backend.DTOs.UserDTOs.UserAccountDTOs;
using Backend.Models.UserModels;

namespace Backend.Services.UserAccountService
{
    public interface IUserAccountService
    {
        Task<UserAccount> ActivateUserAccount(string id);
        Task<UserAccount> BanUserAccount(BanUserAccountDTO banDTO);
        bool CheckUsernameTaken(string username);
        Task<UserAccount> DeactivateUserAccount(string id);
        Task<UserAccount> DeleteUserAccount(string id);
        Task<UserAccount> ReadUserAccountById(string id);
        Task<List<UserAccount>> ReadUserAccounts(FilterUserAccountDTO filterDTO);
        Task<UserAccount> UpdateUserAccount(UpdateUserAccountDTO updateDTO);
    }
}
