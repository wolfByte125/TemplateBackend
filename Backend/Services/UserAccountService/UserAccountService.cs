using AutoMapper;
using Backend.DTOs.UserDTOs.UserAccountDTOs;
using Backend.Models.UserModels;
using Backend.Services.AuthServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System.Security.Cryptography.X509Certificates;

namespace Backend.Services.UserAccountService
{
    public class UserAccountService : IUserAccountService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;

        public UserAccountService(DataContext context, IMapper mapper, 
            IAuthService authService)
        {
            _context = context;
            _mapper = mapper;
            _authService = authService;
        }

        // REGISTER
        public async Task<UserAccount> RegisterUserAccount(RegisterUserAccountDTO registerDTO)
        {
            if (registerDTO.Password.Length < 8)
                throw new InvalidOperationException("Password Is Too Short!");

            CheckUsernameTaken(registerDTO.Username);

            UserAccount userAccount = _mapper.Map<UserAccount>(registerDTO);

            _authService.CreatePasswordHash(registerDTO.Password, out byte[] passwordHash, out byte[] passwordSalt);

            userAccount.Password = "";
            userAccount.PasswordHash = passwordHash;
            userAccount.PasswordSalt = passwordSalt;

            userAccount.Id = GenerateId();

            #region ASSIGN USER ROLE
            UserRole userRole = await _context.UserRoles
                .Where(x => x.RoleName == SEEDED_ROLES.DEFAULT_ROLE)
                .FirstAsync();

            userAccount.UserRoleId = userRole.Id;

            #endregion

            _context.UserAccounts.Add(userAccount);
            await _context.SaveChangesAsync();

            return userAccount;
        }
                
        #region READS
        // READ BY ID
        public async Task<UserAccount> ReadUserAccountById(string id)
        {
            UserAccount? userAccount = await _context.UserAccounts
                .Where(x => x.Id == id)
                .Include(x => x.UserRole.Permissions)
                .FirstOrDefaultAsync()
                ??
                throw new KeyNotFoundException("User Account Not Found.");

            return userAccount;
        }

        // READ ALL
        public async Task<List<UserAccount>> ReadUserAccounts(FilterUserAccountDTO filterDTO)
        {
            List<UserAccount> userAccounts = await _context.UserAccounts
                .Where(x =>
                    (filterDTO.Status == null || x.Status == filterDTO.Status) &&
                    (filterDTO.UserRoleId == null || x.UserRoleId == filterDTO.UserRoleId))
                .Include(x => x.UserRole.Permissions)
                .ToListAsync();

            return userAccounts;
        }
        #endregion

        #region MANAGE USER ACCOUNT

        // UPDATE
        public async Task<UserAccount> UpdateUserAccount(UpdateUserAccountDTO updateDTO)
        {
            UserAccount userAccount = await ReadUserAccountById(updateDTO.Id);

            if (updateDTO.Username != string.Empty &&
                updateDTO.Username != null &&
                userAccount.Username != updateDTO.Username)
            {
                UserRole? userRole = await _context.UserRoles
                    .Where(x => x.Id == updateDTO.UserRoleId)
                    .FirstOrDefaultAsync()
                    ??
                    throw new KeyNotFoundException("User Role Not Found.");

                CheckUsernameTaken(updateDTO.Username);

                _mapper.Map(updateDTO, userAccount);

                _context.UserAccounts.Update(userAccount);
                await _context.SaveChangesAsync();
            }

            return userAccount;
        }

        // DELETE
        public async Task<UserAccount> DeleteUserAccount(string id)
        {
            UserAccount userAccount = await ReadUserAccountById(id: id);

            userAccount.Status = USER_STATUS.DELETED;

            _context.UserAccounts.Update(userAccount);
            await _context.SaveChangesAsync();

            return userAccount;
        }

        // ACTIVATE USER ACCOUNT
        public async Task<UserAccount> ActivateUserAccount(string id)
        {
            UserAccount userAccount = await ReadUserAccountById(id: id);

            userAccount.Status = USER_STATUS.ACTIVE;

            _context.UserAccounts.Update(userAccount);
            await _context.SaveChangesAsync();

            return userAccount;
        }

        // DEACTIVATE USER ACCOUNT
        public async Task<UserAccount> DeactivateUserAccount(string id)
        {
            UserAccount userAccount = await ReadUserAccountById(id: id);

            userAccount.Status = USER_STATUS.INACTIVE;

            _context.UserAccounts.Update(userAccount);
            await _context.SaveChangesAsync();

            return userAccount;
        }

        // BAN USER ACCOUNT
        public async Task<UserAccount> BanUserAccount(BanUserAccountDTO banDTO)
        {
            UserAccount userAccount = await ReadUserAccountById(banDTO.UserAccountId);

            BannedAccount bannedAccount = _mapper.Map<BannedAccount>(banDTO);

            userAccount.CountBans += 1;

            _context.UserAccounts.Update(userAccount);
            _context.BannedAccounts.Add(bannedAccount);
            await _context.SaveChangesAsync();

            return userAccount;
        }

        #endregion

        #region VALIDATORS | GENERATORS

        public bool CheckUsernameTaken(string username)
        {
            UserAccount? existingUser = _context.UserAccounts.FirstOrDefault(u => u.Username == username);

            if (existingUser != null)
                throw new InvalidOperationException("Username Already Exists!");

            return false;
        }

        private string GenerateId()
        {

            var id = Guid.NewGuid().ToString();

            var existingUser = _context.UserAccounts.AsNoTracking().FirstOrDefault(u => u.Id == id);

            if (existingUser != null) // if id exists generate again
                return GenerateId();

            return id;
        }

        #endregion
    }
}
