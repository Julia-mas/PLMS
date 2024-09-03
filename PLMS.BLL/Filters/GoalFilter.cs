namespace PLMS.BLL.Filters
{
    public class GoalFilter : BaseFilter
    {
        public enum GoalSortFields
        {
            CreatedAt,
            DueDate,
            Category,
            Priority,
            Status
        }

        public GoalSortFields SortField { get; set; }

        public IEnumerable<string>? CategoryIds { get; set; }

        public IEnumerable<int>? PriorityIds { get; set; }

        public IEnumerable<int>? StatusIds { get; set; }
    }
}
