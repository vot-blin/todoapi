using System;
using TodoApi.Models;
using TodoApi.Service;
using Microsoft.AspNetCore.Mvc;

namespace TodoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodosController : ControllerBase
    {
        private readonly MongoDBService _mongoDBService;

        public TodosController(MongoDBService mongoDBService)
        {
            _mongoDBService = mongoDBService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoItem>>> Get()
        {
            var todos = await _mongoDBService.GetAsync();
            return Ok(todos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TodoItem>> Get(string id)
        {
            var todo = await _mongoDBService.GetAsync(id);
            if (todo == null)
            {
                return NotFound();
            }
            return Ok(todo);    
        }

        [HttpPost]
        public async Task<ActionResult<TodoItem>> Post([FromBody] TodoItem todo)
        {
            await _mongoDBService.CreateAsync(todo);
            return CreatedAtAction(nameof(Get), new {id = todo.Id}, todo); 
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<TodoItem>> Put(string id, [FromBody] TodoItem updatedTodo)
        {
            var existingTodo = await _mongoDBService.GetAsync(id);
            if (existingTodo == null)
            {
                return NotFound();
            }

            updatedTodo.Id = id;
            updatedTodo.UpdatedAt = DateTime.UtcNow;
            await _mongoDBService.UpdateAsync(id, updatedTodo);
            return NoContent(); 
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(string id)
        {
            var todo = _mongoDBService.GetAsync(id);
            if (todo == null)
            {
                return NotFound();
            }

            await _mongoDBService.RemoveAsync(id);  
            return NoContent();
        }
    }
}
