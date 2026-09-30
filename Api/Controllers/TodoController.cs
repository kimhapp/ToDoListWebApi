using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ToDoListWebApi.Dtos;
using ToDoListWebApi.Models;
using ToDoListWebApi.Services;
using ToDoListWebApi.Utils;

namespace ToDoListWebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/todo")]
    public class ToDoController(IToDoService toDoService) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<List<ToDoDto>>> GetAllByUserId()
        {
            List<ToDo> toDos = await toDoService.GetAllByUserIdAsync(User.GetUserId());

            return toDos.Select(t => t.ToDto()).ToList();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ToDoDto>> GetById(Guid id)
        {
            ToDo? toDo = await toDoService.GetByIdAsync(id, User.GetUserId());
            if (toDo == null) return NotFound();

            return toDo.ToDto();
        }

        [HttpPost]
        public async Task<ActionResult<ToDoDto>> Create(CreateToDoDto createToDoDto)
        {
            ToDo toDo = await toDoService.CreateAsync(User.GetUserId(), createToDoDto.Title, createToDoDto.Description);

            return toDo.ToDto();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, UpdateToDoDto updateToDoDto)
        {
            bool success = await toDoService.UpdateAsync(id, User.GetUserId(), updateToDoDto.Title, updateToDoDto.Description, updateToDoDto.Complete);
            if (!success) return NotFound();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            bool success = await toDoService.RemoveAsync(id, User.GetUserId());
            if (!success) return NotFound();

            return NoContent();
        }
    }   
}   