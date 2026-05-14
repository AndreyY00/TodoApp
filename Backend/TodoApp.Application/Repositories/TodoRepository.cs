using System;
using System.Collections.Generic;
using System.Text;
using TodoApp.Application.Interfaces;
using TodoApp.Domain.Entities;

namespace TodoApp.Application.Repositories
{
    public class TodoRepository : ITodoRepository
    {

        private readonly List<TodoItem> _todos = [];

        public TodoItem AddTask(string task)
        {
            TodoItem newTask = new TodoItem(task);
            _todos.Add(newTask);
            return newTask;

        }

        public void DeleteTask(Guid id)
        {
            var todoIteam = _todos.FirstOrDefault(t=> t.Id == id);
            if(todoIteam is  null)
                throw new ArgumentNullException("Task not found, it was not possible to delete it");
            _todos.Remove(todoIteam);
        }

        public TodoItem GetTaskById(Guid id)
        {
            var task = _todos.FirstOrDefault(t => t.Id == id);
            if(task is null)
                throw new ArgumentNullException("No record found, cannot be updated");

            return task;
        }

        public IEnumerable<TodoItem> GetTodoList()
        {
            return _todos.OrderBy(t => t.CreatedAt);
        }

        public TodoItem UpdateTask(TodoItem todoIteam)
        {
            var todoAux= _todos.FirstOrDefault(t => t.Id == todoIteam.Id);
            if(todoAux is null)
                throw new ArgumentNullException("Task not found, it is not possible to be deleted");

            todoAux.Task = todoIteam.Task;
            todoAux.IsCompleted = todoIteam.IsCompleted;

            return todoAux;
        }
    }
}
