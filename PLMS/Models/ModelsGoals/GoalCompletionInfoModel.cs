namespace PLMS.API.Models.ModelsGoals
{
    public class GoalCompletionInfoModel
    {
        public IEnumerable<GoalInfoModel> Goals { get; set; } = new List<GoalInfoModel>();

        public class GoalInfoModel
        {
            public int GoalId { get; set; }
            public string GoalTitle { get; set; } = string.Empty;
            public double CompletionPercentage { get; set; }
        }
    }
}
