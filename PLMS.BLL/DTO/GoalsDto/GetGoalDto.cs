using PLMS.BLL.DTO.GoalCommentsDto;

namespace PLMS.BLL.DTO.GoalsDto
{
    public class GetGoalDto : GoalBaseDto
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CategoryTitle { get; set; } = string.Empty;
        public string StatusTitle { get; set; } = string.Empty;
        public string PriorityTitle { get; set; } = string.Empty;

        public List<GetGoalCommentDto> GoalComments { get; set; } = new List<GetGoalCommentDto>();
        public List<string> TaskTitles { get; set; } = new List<string>();
    }
}
