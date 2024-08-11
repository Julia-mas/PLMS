namespace PLMS.BLL.DTO
{
    public class EditTaskDto: TaskBaseDto
    {
        public EditGoalDto Goal { get; set; } = new EditGoalDto();
    }
}
