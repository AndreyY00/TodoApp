using System;
using System.Collections.Generic;
using System.Text;
using TodoApp.Application.DTOs;
using TodoApp.Application.Interfaces;
using TodoApp.Application.Repositories;
using TodoApp.Application.Services;
using TodoApp.Domain.Entities;

namespace TodoApp.tests.Service
{
    public class TodoServiceTests
    {

        private readonly ITodoRepository _repository;
        private readonly ITodoService _service;

        public TodoServiceTests()
        {
            _repository = new TodoRepository();
            _service = new TodoService(_repository);
        }

        [Fact]
        public void GetAll_WhenNoTodos_ReturnsEmptyList()
        {
            var result = _service.GetTodoList();

            Assert.Empty(result);
        }

        [Fact]
        public void GetAll_WhenTodosExist_ReturnsAllTodos()
        {
            _service.AddTask("Task 1");
            _service.AddTask("Task 3");

            var result = _service.GetTodoList();

            Assert.Equal(2, result.Count());
        }



        /**********************************GetTaskById*******************************************/

        [Fact]
        public void GetById_WhenTodoExists_ReturnsTodo()
        {
            var newtask =_service.AddTask("Task 1");

            var result = _service.GetTaskById(newtask.Id);

            Assert.NotNull(result);
            Assert.Equal(newtask.Id, result.Id);
            Assert.Equal("Task 1", result.Task);
        }



        /**********************************Add*******************************************/

        [Fact]
        public void Add_WithValidTitle_ReturnsTodoWithCorrectData()
        {
            var result = _service.AddTask("Buy groceries");

            Assert.NotEqual(Guid.Empty, result.Id);
            Assert.Equal("Buy groceries", result.Task);
            Assert.False(result.IsCompleted);
        }

    


        [Fact]
        public void Add_WithEmptyTitle_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() => _service.AddTask(""));
            Assert.Throws<ArgumentException>(() => _service.AddTask("    "));
            Assert.Throws<ArgumentException>(() => _service.AddTask(null));
        }

 

        // ─── Update ────────────────────────────────────────────────────────────

        [Fact]
        public void Update_WithValidData_UpdatesTitleAndCompletion()
        {
            var created = _service.AddTask("firts task");

            var result = _service.UpdateTask(created.Id, new UpdateTodoDTos("New task", true));

            Assert.NotNull(result);
            Assert.Equal("New task", result.Task);
            Assert.True(result.IsCompleted);
        }

        [Fact]
        public void Update_WhenTodoDoesNotExist_ReturnsNull()
        {
            Assert.Throws<ArgumentNullException>(() => _service.UpdateTask(Guid.NewGuid(), new UpdateTodoDTos("New task", false)));

        }


        [Fact]
        public void Update_CanMarkTodoAsCompleted()
        {
            var created = _service.AddTask("Task 1");

            var result = _service.UpdateTask(created.Id, new UpdateTodoDTos(created.Task, true));

            Assert.True(result!.IsCompleted);
        }

 
        // ─── Delete ────────────────────────────────────────────────────────────

        [Fact]
        public void Delete_WhenTodoExists_ReturnsTrue()
        {
            var created = _service.AddTask("Task 1");
            Guid id = created.Id;
            _service.DeleteTask(created.Id);
            Assert.Throws<ArgumentNullException>(() => _service.GetTaskById(id)); //because it does not find the record, and throws an exception, which is the expected behavior
        }




        [Fact]
        public void Delete_OnlyRemovesTargetTodo()
        {
            var first = _service.AddTask("Task 1");
            var second = _service.AddTask("Task 2");

            _service.DeleteTask(first.Id);

            var remaining = _service.GetTodoList();
            Assert.Single(remaining);
            Assert.Equal(second.Id, remaining.First().Id);
        }
    }
}
