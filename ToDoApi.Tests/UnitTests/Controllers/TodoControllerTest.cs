using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ToDoApi.Controllers;
using ToDoApi.Models;
using ToDoApi.Repository;
using Xunit;

namespace ToDoApi.Tests.UnitTests.Controllers
{
    public class TodoControllerTest
    {
        [Fact]
        public void Index_Returns_ListOf_Todo()
        {
            // Arrange
            var mockRepo = new Mock<ITodoRepository>();
            mockRepo.Setup(repo => repo.GetAll())
                .Returns(GetTestTodos());

            var controller = new TodoController(mockRepo.Object);

            // Act
            var result = controller.GetAll();

            // Assert
            Assert.Equal(2, result.Count());
            Assert.Equal("Test One", result.FirstOrDefault().Name);
        }

        [Fact]
        public void Create_ReturnsBadRequest_GivenInvalidName()
        {
            // Arrange & Act
            var mockRepo = new Mock<ITodoRepository>();
            var controller = new TodoController(mockRepo.Object);
            controller.ModelState.AddModelError("error", "some error");

            // Act
            var result = controller.Create(todo: new Todo {Name = "ไม่ถูกไม่ควร"});

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public void Create_ReturnsBadRequest_GivenNullModel()
        {
            // Arrange & Act
            var mockRepo = new Mock<ITodoRepository>();
            var controller = new TodoController(mockRepo.Object);
            controller.ModelState.AddModelError("error", "some error");

            // Act
            var result = controller.Create(todo: null);

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public void Create_ReturnsCreatedTodo_GivenCorrectInputs()
        {
            // Arrange
            const string testName = "Test Name";
            var newTodo = new Todo
            {
                Name = testName
            };
            var mockRepo = new Mock<ITodoRepository>();
            var controller = new TodoController(mockRepo.Object);

            // Act
            var result = controller.Create(newTodo);

            // Assert
            var okResult = Assert.IsType<CreatedAtRouteResult>(result);
            var returnTodo = Assert.IsType<Todo>(okResult.Value);
            Assert.Equal(0, returnTodo.TodoItems.Count);
            Assert.Equal(testName, returnTodo.Name);
        }

        [Fact]
        public void Update_ReturnsBadRequest_GivenNullModel()
        {
            // Arrange & Act
            const int testTodoId = 123;
            var mockRepo = new Mock<ITodoRepository>();
            var controller = new TodoController(mockRepo.Object);
            controller.ModelState.AddModelError("error", "some error");

            // Act
            var result = controller.Update(testTodoId, payload: null);

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public void Update_ReturnsBadRequest_GivenInvalidName()
        {
            // Arrange & Act
            const int testTodoId = 123;
            var mockRepo = new Mock<ITodoRepository>();
            var controller = new TodoController(mockRepo.Object);
            controller.ModelState.AddModelError("error", "some error");

            // Act
            var result = controller.Update(testTodoId, payload: new Todo {Name = "ไม่ถูกไม่ควร"});

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public void Update_ReturnsHttpNotFound_ForInvalidId()
        {
            // Arrange
            const int testTodoId = 123;
            var mockRepo = new Mock<ITodoRepository>();
            mockRepo.Setup(repo => repo.Find(testTodoId))
                .Returns((Todo) null);
            var controller = new TodoController(mockRepo.Object);

            // Act
            var result = controller.Update(testTodoId, new Todo {Name = "OK"});

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void Update_ReturnsUpdatedTodo_GivenCorrectInputs()
        {
            // Arrange
            const int testTodoId = 123;
            const string testName = "Updated";
            var oldTodo = new Todo
            {
                Name = "Todo"
            };
            var updateTodo = new Todo
            {
                Name = testName
            };
            var mockRepo = new Mock<ITodoRepository>();
            mockRepo.Setup(repo => repo.Find(testTodoId))
                .Returns(oldTodo);
            var controller = new TodoController(mockRepo.Object);

            // Act
            var result = controller.Update(testTodoId, updateTodo);

            // Assert
            var okResult = Assert.IsType<CreatedAtRouteResult>(result);
            var returnTodo = Assert.IsType<Todo>(okResult.Value);
            Assert.Equal(testName, returnTodo.Name);
        }

        private static IEnumerable<Todo> GetTestTodos()
        {
            return new List<Todo>
            {
                new Todo
                {
                    Id = 1,
                    Name = "Test One",
                    TodoItems = new List<TodoItem>()
                },
                new Todo
                {
                    Id = 2,
                    Name = "Test Two",
                    TodoItems = new List<TodoItem>()
                }
            };
        }
    }
}