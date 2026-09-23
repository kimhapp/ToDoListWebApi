using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToDoListWebApi.Migrations;
using ToDoListWebApi.Models;

namespace ToDoListWebApi.Controllers
{
    [ApiController]
    [Route("api/todo")]
    public class ToDoController(ApplicationDbContext context) : ControllerBase
    {
        private readonly ApplicationDbContext _context = context;

            [HttpGet]
        public async Task<ActionResult<IEnumerable<ToDo>>> GetAll()
        {
            return await _context.ToDos.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ToDo>> GetById(int id)
        {
            ToDo? toDo = await _context.ToDos.FindAsync(id);

            if (toDo == null) return NotFound();
            return toDo;
        }

        [HttpPost]
        public async Task<ActionResult<ToDo>> Create([Bind("Title,Description")] ToDo toDo)
        {
            _context.ToDos.Add(toDo);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = toDo.Id}, toDo);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ToDo>> Update(int id, [Bind("Title,Description,Complete")] ToDo toDo)
        {
            ToDo? existingToDo = await _context.ToDos.FindAsync(id);
            if (existingToDo == null) return NotFound();

            existingToDo.Title = toDo.Title;
            existingToDo.Description = toDo.Description;
            existingToDo.Complete = toDo.Complete;
            
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ToDo>> Delete(int id)
        {
            ToDo? existingToDo = await _context.ToDos.FindAsync(id);
            if (existingToDo == null) return NotFound();

            _context.ToDos.Remove(existingToDo);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }   
}   