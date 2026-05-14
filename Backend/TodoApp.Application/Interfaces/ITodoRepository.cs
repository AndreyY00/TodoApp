using System;
using System.Collections.Generic;
using System.Text;
using TodoApp.Domain.Entities;

namespace TodoApp.Application.Interfaces
{
    public interface ITodoRepository
    {
        public TodoItem AddTask(string task);
        public void DeleteTask(Guid id);
        public TodoItem UpdateTask(TodoItem todoIteam);
        public IEnumerable<TodoItem> GetTodoList();
        public TodoItem GetTaskById(Guid id);


    }
}
