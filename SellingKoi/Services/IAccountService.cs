using SellingKoi.Models;

namespace SellingKoi.Services
{
    public interface IAccountService
    {
        //user
        Task<Account> LoginAsync(string username, string password);
        Task<bool> RegisterAsync(string username, string password, string fullname);
        Task LogoutAsync();
        Task<bool> ChangePasswordAsync(string userId, string currentPassword, string newPassword);
        Task<IEnumerable<Account>> GetAllAccountsAsync();
        Task<List<Account>> SearchlistStaffbylistId(List<string> listaccountid);

        //Task<bool> UpdateProfileAsync(string userId, UserProfileModel model);

        //admin
        Task<bool> AssignRoleToUserAsync(string userId, string role);
        Task<Account> GetAccountByIdAsync(Guid id);
        Task NegateAccountAsync(Guid id);
        Task<List<Account>> GetStaffAccountAsync();
        Task<List<Account>> GetSaleStaffAccountAsync();
        Task UnassignTripFromSaleStaffAsync(string accountId);
        Task UnassignTripFromFollowStaffAsync(List<string> accountIds);
        Task<List<Account>> GetSaleStaffWithNoTrip();
        Task<List<Account>> GetStaffWithNoTrip();
        Task<Account> GetSaleStaffByTripIdAsync(string tripId);
        Task<List<Account>> GetStaffByTripIdAsync(string tripId);

    }
}
