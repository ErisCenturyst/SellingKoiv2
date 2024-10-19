using SellingKoi.Models;

namespace SellingKoi.Services
{
    public interface IKoiService
    {
        Task<IEnumerable<KOI>> GetAllKoisAsync();
        Task<KOI> GetKoiByIdAsync(Guid id);
        Task<Guid?> GetIdByNameAsync(string name);
        Task CreateKoiAsync(KOI Koi);
        Task UpdateKoiAsync(KOI Koi);
        Task NegateKoiAsync(Guid id);
        Task<Pagination<KOI>> GetKOIsPaged(int page, int limit);
    }
}
