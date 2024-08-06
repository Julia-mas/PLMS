﻿using PLMS.API.Models.ModelsTasks;

namespace PLMS.API.Models
{
    public class AddGoalModel
    {
        public string Title { get; set; } = null!;
        public string Description { get; set; } = string.Empty;
        public DateTime DueDate { get; set; }
        public int StatusId { get; set; }
        public int PriorityId { get; set; }
        public int CategoryId { get; set; }
        public ICollection<AddTaskModel>? Tasks { get; set; }
        public ICollection<GoalCommentModel>? GoalComments { get; set; }
        public AddCategoryModel? Category { get; set; }
    }
}
