namespace PLMS.BLL.Filters
{
    public class TaskFilter: BaseFilter
    {
        public enum TaskSortFields
        {
            CreatedAt,
            DueDate,
            Goal,
            Category,
            Priority,
            Status
        }

        public TaskSortFields SortField { get; set; }

        public IEnumerable<string>? CategoryIds { get; set; }

        public IEnumerable<string>? GoalIds { get; set; }

        public IEnumerable<int>? PriorityIds { get; set;}

        public IEnumerable<int>? StatusIds { get; set;}
    }
}
