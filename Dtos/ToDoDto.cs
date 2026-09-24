using System.ComponentModel.DataAnnotations;

namespace ToDoListWebApi.Dtos
{
    public class ToDoDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = "";
        public string Description { get; set; } = "";
        public bool Complete { get; set; } = false;
        public DateTime CreatedAt { get; set; } 
        public DateTime UpdatedAt { get; set; } 
    }

    public class CreateToDoDto
    {
        [Required]
        [StringLength(50)]
        public string Title { get; set; } = "";

        [StringLength(250)]
        public string Description { get; set; } = "";
    }

    public class UpdateToDoDto
    {
        [Required]
        [StringLength(50)]
        public string Title { get; set; } = "";

        [StringLength(250)]
        public string Description { get; set; } = "";

        public bool Complete { get; set; } = false;
    }
}