using Microsoft.EntityFrameworkCore;
using SellingKoi.Data;
using SellingKoi.Models;

namespace SellingKoi.Services
{
    public class AccountService : IAccountService
    {
        private readonly DataContext _dataContext;
        public AccountService(DataContext context)  
        {
            _dataContext = context;
        }

        public async Task<Account> GetAccountByIdAsync(Guid id)
        {
            return await _dataContext.Accounts.Where(a => a.Status).FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<IEnumerable<Account>> GetAllAccountsAsync()
        {
            return await _dataContext.Accounts.Where(a => a.Status).ToListAsync();
        }

        public async Task NegateAccountAsync(Guid id)
        {
            var account = await _dataContext.Accounts.FindAsync(id);
            if (account != null)
            {
                try
                {
                    account.Status = false;
                    _dataContext.Accounts.Update(account);
                    await _dataContext.SaveChangesAsync();

                }
                catch (Exception ex)
                {
                    // Log the exception
                    throw new Exception("An error occurred while deactivating the farm.", ex);
                }
            }
            else
            {
                throw new KeyNotFoundException($"Farm with ID {id} not found.");
            }
        }

        public async Task UpdateAccountAsync(Account account)
        {
            _dataContext.Entry(account).State = EntityState.Modified;
            await _dataContext.SaveChangesAsync();
        }



        public async Task<bool> RegisterAsync(string username, string password, string fullname)
        {
            if (_dataContext.Accounts.Any(a => a.Username == username))
            {
                return await Task.FromResult(false);
            }

            var account = new Account
            {
                Username = username,
                Password =password,
                Fullname = fullname,
                Role = "Customer"
            };

            _dataContext.Accounts.Add(account);
            _dataContext.SaveChanges();
            return await Task.FromResult(true);
        }

        public async Task<Account> LoginAsync(string username, string password)
        {
            var account = await _dataContext.Accounts.FirstOrDefaultAsync(a => a.Username == username);
            if (account == null)
            {
                return null;
            }
            return account;
        }

        public Task LogoutAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<bool> AssignRoleToUserAsync(string userId, string role)
        {
            var account = await _dataContext.Accounts.FindAsync(userId);
            if (account == null) return false;

            account.Role = role;
            await _dataContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ChangePasswordAsync(string userId, string currentPassword, string newPassword)
        {
            var account = await _dataContext.Accounts.FindAsync(userId);
            if (account == null)
            {
                return false;
            }

            account.Password = newPassword;
            await _dataContext.SaveChangesAsync();
            return true;
        }

    }
}
