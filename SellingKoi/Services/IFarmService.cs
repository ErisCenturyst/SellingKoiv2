using SellingKoi.Models;

namespace SellingKoi.Services
{
    public interface IFarmService
    {
        Task<IEnumerable<Farm>> GetAllFarmsAsync();
        Task<Farm> GetFarmByIdAsync(string id);
        Task<Guid?> GetIdByNameAsync(string name);
        Task CreateFarmAsync(Farm farm);
        Task UpdateFarmAsync(Farm farm);
        Task NegateFarmAsync(Guid id);

    }
}
