using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TaskManagement.Core.Enums;

namespace TaskManagement.Core.Models
{
    /*
     Title (required) 


§
Description (optional) 


§
Due Date (optional) 


§
Priority (low, medium, high) 


§
Status (open, in progress, completed) 


§
Assignee (optional) - User who is assigned the task */
    public class Task
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        public string? Description { get; set; }

        [Column(TypeName = "date")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]//yyyy-MM-dd
        public DateTime DueDate { get; set; }

        public Priority Priority { get; set; }

        public Status Status { get; set; }

        // UserName of the User Identity object
        public string? Assignee { get; set; }

        [ForeignKey("User")]
        // Foreign key property (Id) referencing the User 
        public string? UserId { get; set; }


        // Navigation property to the User Identity object
        [InverseProperty("Tasks")]
        public ApplicationUser? User { get; set; }

        [NotMapped]
        public ICollection<ApplicationUser>? Users { get; set; }
    }
}
