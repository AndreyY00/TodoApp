using TodoApp.Application;
using TodoApp.Application.DTOs;
using TodoApp.Domain.Entities;

namespace TodoApp.Application.Interfaces
{
    public interface ITodoService
    {
        public TodoItem AddTask(string todoIteam);
        public void DeleteTask(Guid Id);
        public TodoItem UpdateTask(Guid id, UpdateTodoDTos todoIteam);
        public IEnumerable<TodoItem> GetTodoList();
        public TodoItem GetTaskById(Guid id);
    }
}
