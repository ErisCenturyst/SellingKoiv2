namespace SellingKoi.Models
{
    public class TripTask //task of trip
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public required bool Status { get; set; } = false;
        public required string TripId { get; set; }
        public required string StaffId { get; set; }
        public required string StaffName { get; set; }
    }
}
