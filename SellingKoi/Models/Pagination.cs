namespace SellingKoi.Models
{
    public class Pagination<T>
    {
        public int Page { get; set; }
        public int Limit { get; set; }
        public List<T> Result { get; set; }
        public int TotalPage { get; set; }
        public int TotalItem { get; set; }
        public bool IsNextPage { get; set; }
        public bool IsPrePage { get; set; }
    }
}
