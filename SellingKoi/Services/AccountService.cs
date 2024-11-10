using Google.Apis.Requests;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;
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
        public async Task<Account> GetSaleStaffByTripIdAsync(string tripId)
        {
             return await _dataContext.Accounts
                .Where(a => a.Role.Equals("SaleStaff") && a.Status && a.TripId == tripId) // Kiểm tra các điều kiện
                .SingleOrDefaultAsync(); // Chuyển đổi kết 
        }

        public async Task<List<Account>> GetStaffByTripIdAsync(string tripId)
        {
            return await _dataContext.Accounts
                .Where(a => a.Role.Equals("Staff") && a.Status && a.TripId == tripId.ToUpper()) // Kiểm tra các điều kiện
                .ToListAsync(); // Chuyển đổi kết quả thành danh sách
        }

        public async Task<List<Account>> GetSaleStaffWithNoTrip()
        {
            return await _dataContext.Accounts
                .Where(a => a.Status && a.Role.Equals("SaleStaff") && a.TripId == null) // Kiểm tra các điều kiện
                .ToListAsync(); // Chuyển đổi kết quả thành danh sách
        }
        public async Task<List<Account>> GetStaffWithNoTrip()
        {
            return await _dataContext.Accounts
                .Where(a => a.Status && a.Role.Equals("Staff") && a.TripId == null) // Kiểm tra các điều kiện
                .ToListAsync(); // Chuyển đổi kết quả thành danh sách
        }
        public async Task<Account> GetAccountByIdAsync(Guid id)
        {
            return await _dataContext.Accounts.Where(a => a.Status).FirstOrDefaultAsync(a => a.Id == id);
        }
        public async Task<List<Account>> GetStaffAccountAsync()
        {
            return await _dataContext.Accounts.Where(a => a.Role.Equals("Staff") && a.Status).ToListAsync();
        }
        public async Task<List<Account>> GetSaleStaffAccountAsync()
        {
            return await _dataContext.Accounts.Where(a => a.Role.Equals("SaleStaff") && a.Status).ToListAsync();
        }
        public async Task<List<Account>> SearchlistStaffbylistId(List<string> listaccountid)
        {
            return await _dataContext.Accounts
                .Where(a => a.Role.Equals("Staff") && listaccountid.Contains(a.Id.ToString()))
                .ToListAsync();
        }
        public async Task UnassignTripFromSaleStaffAsync(string accountId)
        {
            if (string.IsNullOrEmpty(accountId))
            {
                throw new ArgumentException("Account ID cannot be null or empty", nameof(accountId));
            }

            var account = await _dataContext.Accounts.FindAsync(Guid.Parse(accountId));
            if (account == null)
            {
                throw new ArgumentException("Account not found", nameof(account));
            }

            account.TripId = null;
            await _dataContext.SaveChangesAsync();
        }

        public async Task UnassignTripFromFollowStaffAsync(List<string> accountIds)
        {
            if (accountIds == null || !accountIds.Any())
            {
                throw new ArgumentException("Account IDs cannot be null or empty", nameof(accountIds));
            }

            var accounts = await _dataContext.Accounts
            .Where(a => accountIds.Contains(a.Id.ToString())) // Lấy tất cả tài khoản có trong danh sách accountIds
                .ToListAsync();

            if (accounts.Count == 0)
            {
                throw new ArgumentException("No accounts found for the provided IDs", nameof(accountIds));
            }

            foreach (var account in accounts)
            {
                account.TripId = null; // Gỡ bỏ tripId
            }

            await _dataContext.SaveChangesAsync(); // Lưu thay đổi vào cơ sở dữ liệu
        }


        public async Task<IEnumerable<Account>> GetAllAccountsAsync()
        {
            return await _dataContext.Accounts.Where(a => a.Status).ToListAsync();
        }

        //public async Task<List<Account>> GetFreeSaleStaffAccountAsync()
        //{
            
        //}

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
                Password = password,
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
            var account = await _dataContext.Accounts.FindAsync(Guid.Parse(userId.ToUpper()));
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
