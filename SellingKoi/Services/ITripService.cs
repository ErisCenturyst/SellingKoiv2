using SellingKoi.Models;

namespace SellingKoi.Services
{
    public interface ITripService
    {
        Task<IEnumerable<Trip>> GetAllTripAsync();
        Task<Trip> GetTripByIdAsync(string id);
        Task CreateTripAsync(Trip Koi);
        Task UpdateTripAsync(Trip Koi);
        Task UnasignListStaffsAsync(string id);
        Task UnasignSaleStaffAsync(string id);
        Task DeleteTripAsync(string id);
        //Task NegateKoiAsync(Guid id);
        //Task<Pagination<KOI>> GetKOIsPaged(int page, int limit);
    }
}

