namespace ToDoListWebApi.Models
{
    public class ToDo
    {
        public int Id { get; set; }
        public required string Title { get; set; } 
        public string Description { get; set; } = "";
        public bool Complete { get; set; } = false;
        public DateTime TimeStamp { get; set; } = DateTime.UtcNow;
    }
}