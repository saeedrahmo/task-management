using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using TaskManagement.Core.Enums;

namespace TaskManagement.Core.DTOs
{
    public class UpdateTaskDto
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        public string? Description { get; set; }

        [Column(TypeName = "date")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]//yyyy-MM-dd
        public DateTime DueDate { get; set; }

        public Priority Priority { get; set; }

        public Status Status { get; set; }

        public string? Assignee { get; set; }

        public IEnumerable<UserListDto>? Users { get; set; }
    }
}
