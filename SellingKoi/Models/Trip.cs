namespace SellingKoi.Models
{
    public class Trip
    {
        public Guid Id { get; set; }
        public int TripNum { get; set; }
        public string status { get; set; }
        //Preparing, Going, Done
        public DateTime Registration_date { get; set; } = DateTime.Now;
        public string? Price { get; set; }
        public DateTime? Date_of_departure { get; set; }


        //
        public List<string>? OrderShortensID { get; set; }
        public Account? SaleStaff { get; set; }
        // Thêm danh sách các tài khoản tham gia chuyến đi
        public List<string>? FollowAccountsID { get; set; } = new List<string>();
    }
}
