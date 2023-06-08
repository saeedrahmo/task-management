using TaskManagement.Core.Enums;

namespace TaskManagement.Core.DTOs
{
    public class ReadTaskDto
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string? Description { get; set; }
        
        public DateTime DueDate { get; set; }

        public Priority Priority { get; set; }

        public Status Status { get; set; }

        public string? Assignee { get; set; }
    }
}
