using Microsoft.AspNetCore.Mvc;
using TodoApp.Application.DTOs;
using TodoApp.Application.Interfaces;
using TodoApp.Domain.Entities;

namespace TodoApp.API.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class TodoController : ControllerBase
    {
        private readonly ITodoService _todoService;
        public TodoController(ITodoService todoService)     
        {
            _todoService = todoService;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var result = _todoService.GetTodoList();
            return Ok(result);
        }


        [HttpPost]
        public IActionResult CreateTask(string Task)
        {
            return Ok(_todoService.AddTask(Task));
        }


        [HttpPut("{id:guid}")]
        public ActionResult<TodoItem> UpdateTask (Guid id,[FromBody] UpdateTodoDTos TodoDto)
        {
            
            return Ok(_todoService.UpdateTask(id, TodoDto));
        }

        [HttpDelete("{id:guid}")]
        public IActionResult DeleteTask(Guid id)
        {
          _todoService.DeleteTask(id);
           return NoContent();
        }

        [HttpGet("{id:guid}")]
        public ActionResult<TodoItem> GetById(Guid id)
        {
            var todo = _todoService.GetTaskById(id);
            return todo is null ? NotFound() : Ok(todo);
        }
    }
}
