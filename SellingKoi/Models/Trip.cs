namespace SellingKoi.Models
{
    public class Trip
    {
        public Guid Id { get; set; }
        public int TripNum { get; set; }
        public List<OrderShorten> ?orders { get; set; }
        public string status { get; set; }
        //preparing, going, done
        public DateTime Registration_date { get; set; } = DateTime.Now;
        public Account ?staff { get; set; }
        public List<OrderShorten> ?ordershortens { get; set; } = new List<OrderShorten>();

    }
}
