namespace PLMS.BLL.DTO.GoalsDto
{
    public class GoalCompletionInfoDto
    {
        public IEnumerable<GoalInfoDto> Goals { get; set; } = new List<GoalInfoDto>();

        public class GoalInfoDto
        {
            public int GoalId { get; set; } 
            public string GoalTitle { get; set; } = string.Empty;
            public double CompletionPercentage { get; set; }
        }
    }
}
