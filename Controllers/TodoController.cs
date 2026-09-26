using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToDoListWebApi.Models;
using ToDoListWebApi.Services;

namespace ToDoListWebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/todo")]
    public class ToDoController(IToDoService service) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ToDo>>> GetAllByUserId(Guid userId)
        {
            IEnumerable<ToDo> toDos = await service.GetAllByUserIdAsync(userId);
            return Ok(toDos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ToDo>> GetById(Guid id, Guid userId)
        {
            ToDo? toDo = await service.GetByIdAsync(id);

            if (toDo == null) return NotFound();
            return toDo;
        }

        [HttpPost]
        public async Task<ActionResult<ToDo>> Create([Bind("Title,Description")] ToDo toDo)
        {
            service.ToDos.Add(toDo);
            await service.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = toDo.Id}, toDo);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ToDo>> Update(int id, [Bind("Title,Description,Complete")] ToDo toDo)
        {
            ToDo? existingToDo = await service.ToDos.FindAsync(id);
            if (existingToDo == null) return NotFound();

            existingToDo.Title = toDo.Title;
            existingToDo.Description = toDo.Description;
            existingToDo.Complete = toDo.Complete;
            
            await service.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ToDo>> Delete(int id)
        {
            ToDo? existingToDo = await service.ToDos.FindAsync(id);
            if (existingToDo == null) return NotFound();

            service.ToDos.Remove(existingToDo);
            await service.SaveChangesAsync();
            return NoContent();
        }
    }   
}   