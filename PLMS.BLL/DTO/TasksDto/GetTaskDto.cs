namespace PLMS.BLL.DTO.TasksDto
{
    public class GetTaskDto: TaskBaseDto
    {
        public DateTime CreatedAt { get; set; }
        public string GoalTitle { get; set; } = string.Empty;
        public string StatusTitle { get; set; } = string.Empty;
        public string PriorityTitle {  get; set; } = string.Empty;  
    }
}

