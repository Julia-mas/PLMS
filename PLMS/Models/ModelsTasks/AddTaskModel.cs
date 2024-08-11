namespace PLMS.API.Models.ModelsTasks
{
    public class AddTaskModel: TaskModelBase
    {
        public AddGoalModel Goal { get; set; } = new AddGoalModel();
    }
}
