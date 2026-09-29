namespace ToDoListWebApi.Models
{
    public class ToDo
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = "";
        public string Description { get; set; } = "";
        public bool Complete { get; set; } = false;
        public DateTime CreatedAt { get; set; } 
        public DateTime UpdatedAt { get; set; } 

        public Guid UserId { get; set; }
        public User User { get; set; } = null!;
    }
}