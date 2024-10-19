using Microsoft.EntityFrameworkCore;
using SellingKoi.Data;
using SellingKoi.Models;

namespace SellingKoi.Services
{

    public class FarmService : IFarmService
    {
        private readonly DataContext _dataContext;
        public FarmService(DataContext dataContext) {
            _dataContext = dataContext;}

        public async Task CreateFarmAsync(Farm farm)
        {
            _dataContext.Farms.Add(farm);
            await _dataContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Farm>> GetAllFarmsAsync()
        {
            return await _dataContext.Farms.Where(f => f.Status).ToListAsync();
            //return await _dataContext.Farms.ToListAsync();
        }

        public async Task<Farm> GetFarmByIdAsync(string id)
        {
            //return await _dataContext.Farms.Where(f => f.Status).FirstAsync(id);
            return await _dataContext.Farms.Include(f => f.KOIs).Where(f=>f.Status).FirstOrDefaultAsync(f => f.Id.ToString() == id);
        }

        public Task<Guid?> GetIdByNameAsync(string name)
        {
            throw new NotImplementedException();
        }

        public async Task NegateFarmAsync(Guid id)
        {
            var farm = await _dataContext.Farms.FindAsync(id);
            if (farm != null)
            {
                try
                {
                    farm.Status = false;
                    _dataContext.Farms.Update(farm);
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

        public async Task UpdateFarmAsync(Farm farm)
        {
            _dataContext.Entry(farm).State = EntityState.Modified;
            await _dataContext.SaveChangesAsync();
        }
    }
}
