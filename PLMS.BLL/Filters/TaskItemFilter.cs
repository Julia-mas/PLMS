namespace PLMS.BLL.Filters
{
    public class TaskItemFilter: BaseFilter
    {
        public enum TaskItemSortFields
        {
            CreatedAt,
            DueDate,
            Goal,
            Category,
            Priority,
            Status
        }

        public TaskItemSortFields SortField { get; set; }

        public IEnumerable<string>? CategoryIds { get; set; }

        public IEnumerable<string>? GoalIds { get; set; }

        public IEnumerable<int>? PriorityIds { get; set;}

        public IEnumerable<int>? StatusIds { get; set;}
    }
}
