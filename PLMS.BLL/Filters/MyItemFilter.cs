namespace PLMS.BLL.Filters
{
    //TODO: this should be a specific filter like TaskTilter
    //because all the fields are related to the Task entity;
    public class MyItemFilter: BaseFilter
    {
        public enum MyItemSortFields
        {
            CreatedAt,
            DueDate,
            Goal,
            Category,
            Priority,
            Status
        }

        public MyItemSortFields SortField { get; set; }

        public IEnumerable<string> CategoryIds { get; set; } = Enumerable.Empty<string>();

        public IEnumerable<string> GoalIds { get; set; } = Enumerable.Empty<string>();

        public IEnumerable<int> PriorityIds { get; set;} = Enumerable.Empty<int>();

        public IEnumerable<int> StatusIds { get; set;} = Enumerable.Empty<int>();
    }
}
