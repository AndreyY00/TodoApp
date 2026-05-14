

using TodoApp.Application.DTOs;
using TodoApp.Application.Interfaces;
using TodoApp.Domain.Entities;

namespace TodoApp.Application.Services
{
    public class TodoService : ITodoService
    {
        private  readonly ITodoRepository _todoRepository;

        public TodoService(ITodoRepository todoRepository) {

            _todoRepository = todoRepository;
        }
           
        public TodoItem AddTask(string task)
        {
            return _todoRepository.AddTask(task);
        }

        public void DeleteTask(Guid todoIteam)
        {
            _todoRepository.DeleteTask(todoIteam);
        }

        public TodoItem GetTaskById(Guid id)
        {
            return _todoRepository.GetTaskById(id);
        }

        public IEnumerable<TodoItem> GetTodoList()
        {
            return _todoRepository.GetTodoList();
        }

        public TodoItem UpdateTask(Guid id, UpdateTodoDTos todoIteam)
        {

            var todoAux = _todoRepository.GetTaskById(id);
            if( todoAux is null || string.IsNullOrEmpty(todoIteam.task))
                throw new ArgumentNullException("The task was not found or the record you are trying to update is empty.");
            todoAux.Task = todoIteam.task;
            todoAux.IsCompleted = todoIteam.isCompleted;
            return todoAux;

        }

    }
}
