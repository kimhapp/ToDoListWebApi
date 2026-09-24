using System.ComponentModel.DataAnnotations;

namespace ToDoListWebApi.Dtos
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = "";
        public string Email { get; set; } = "";
    }

    public class RegisterUserDto
    {
        [Required]
        [StringLength(20, MinimumLength = 3)]
        public string Name { get; set; } = "";

        [Required]
        [EmailAddress]
        public string Email { get; set; } = "";

        [Required]
        [MinLength(8)]
        public string Password { get; set; } = "";
    }

    public class LoginUserDto
    {
        [Required]
        [StringLength(20, MinimumLength = 3)]
        public string Name { get; set; } = "";

        [Required]
        [MinLength(8)]
        public string Password { get; set; } = "";
    }
}