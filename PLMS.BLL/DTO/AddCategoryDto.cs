namespace PLMS.BLL.DTO
{
    public class AddCategoryDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string UserId { get; set; } = null!;
        public ICollection<AddGoalDto> Goals { get; set; } = new List<AddGoalDto>();
    }
}
