using PLMS.BLL.DTO.CategiesDto;

namespace PLMS.BLL.DTO.CategoriesDto
{
    public class GetCategoryDto : CategoryBaseDto
    {
        public int Id { get; set; }
        public string UserId { get; set; } = null!;

        public List<Goal> Goals { get; set; } = new List<Goal>();

        public class Goal
        { 
            public int Id { get; set; }

            public string Title { get; set; } = string.Empty;
        }
    }
}
