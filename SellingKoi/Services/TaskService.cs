using Microsoft.EntityFrameworkCore;
using SellingKoi.Data;
using SellingKoi.Models;

namespace SellingKoi.Services
{
    public class TaskService : ITaskService
    {
        private readonly DataContext _dataContext;

        public TaskService(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task CreateTask(TripTask task)
        {
            _dataContext.Tasks.Add(task);
            await _dataContext.SaveChangesAsync();
        }

        public async Task<List<TripTask>> GetAllTask()
        {
            return await _dataContext.Tasks.ToListAsync();
        }
        public async Task<List<TripTask>> GetAllTaskOfStaff(string staffId)
        {
            // Kiểm tra xem staffId có hợp lệ không
            if (string.IsNullOrEmpty(staffId))
            {
                return new List<TripTask>(); // Trả về danh sách rỗng nếu staffId không hợp lệ
            }

            // Truy vấn cơ sở dữ liệu để lấy tất cả task có staffId tương ứng
            return await _dataContext.Tasks
                .Where(task => task.StaffId == staffId) // Giả sử TripTask có thuộc tính StaffId
                .ToListAsync();
        }

        public async Task<List<TripTask>> GetALlTaskOfStrip(string tripid)
        {
            // Kiểm tra xem staffId có hợp lệ không
            if (string.IsNullOrEmpty(tripid))
            {
                return new List<TripTask>(); // Trả về danh sách rỗng nếu staffId không hợp lệ
            }

            // Truy vấn cơ sở dữ liệu để lấy tất cả task có staffId tương ứng
            return await _dataContext.Tasks
                .Where(task => task.TripId == tripid) // Giả sử TripTask có thuộc tính StaffId
                .ToListAsync(); ;
        }

        public async Task<TripTask> GetTaskById(string id)
        {
            return await _dataContext.Tasks.FirstOrDefaultAsync(t => t.Id.ToString().ToUpper().Equals(id));
        }

        public async Task UpdateStatusTask(TripTask task)
        {
            _dataContext.Entry(task).State = EntityState.Modified;
            await _dataContext.SaveChangesAsync();
        }
    }
}
