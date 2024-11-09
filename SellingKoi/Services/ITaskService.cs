using SellingKoi.Models;

namespace SellingKoi.Services
{
    public interface ITaskService
    {
        Task<List<TripTask>> GetAllTask();
        Task<List<TripTask>> GetAllTaskOfStaff(string staffId);
        Task<List<TripTask>> GetALlTaskOfStrip(string tripid);
        Task<TripTask> GetTaskById (string id);
        Task CreateTask (TripTask task);
        Task UpdateStatusTask (TripTask task);
    }
}
