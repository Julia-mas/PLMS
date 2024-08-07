namespace PLMS.BLL.DTO
{
    public class CategoryDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string UserId { get; set; } = null!;
        public ICollection<EditGoalDto> Goals { get; set; } = new List<EditGoalDto>();
    }
}
