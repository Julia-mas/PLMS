﻿namespace PLMS.BLL.DTO
{
    public class TaskCommentDto
    {
        public int Id { get; set; }
        public string? Comment { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public int TaskId { get; set; }
    }
}
