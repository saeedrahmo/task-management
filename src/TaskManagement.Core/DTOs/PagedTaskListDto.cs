using TaskManagement.Core.Enums;

namespace TaskManagement.Core.DTOs
{
    public class PagedTaskListDto
    {        
        public int Id { get; set; }

        public string Title { get; set; } 

        public string? Description { get; set; }
        
        public DateTime DueDate { get; set; }

        public Priority Priority { get; set; }

        public Status Status { get; set; }
        
        public string? Assignee { get; set; }

        public string SortOrder { get; set; }

        public int PageNumber { get; set; }

        public int PageSize { get; set; }        
    }
}
