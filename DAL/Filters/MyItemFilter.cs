using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLMS.DAL.Filters
{
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
