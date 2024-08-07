namespace PLMS.BLL.Filters
{
    public class BaseFilter
    {
        public enum SortOrders
        {
            Asc = 0,
            Desc
        }

        public int PageNumber { get; set; } = 1;

        public int ItemsPerPageCount { get; set; } = 10;

        public string SearchString { get; set; } = string.Empty;

        public SortOrders SortOrder { get; set; }

        public string UserId { get; set; } = string.Empty;
    }
}

