using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ToDoApi.Controllers;
using ToDoApi.Models;
using ToDoApi.Models.Repository;
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
        public void Update_ReturnsBadRequest_GivenNullModel()
        {
            // Arrange & Act
            const int testSessionId = 123;
            var mockRepo = new Mock<ITodoRepository>();
            var controller = new TodoController(mockRepo.Object);
            controller.ModelState.AddModelError("error", "some error");

            // Act
            var result = controller.Update(testSessionId, payload: null);

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public void Update_ReturnsBadRequest_GivenInvalidName()
        {
            // Arrange & Act
            const int testSessionId = 123;
            var mockRepo = new Mock<ITodoRepository>();
            var controller = new TodoController(mockRepo.Object);
            controller.ModelState.AddModelError("error", "some error");

            // Act
            var result = controller.Update(testSessionId, payload: new Todo {Name = "ไม่ถูกไม่ควร"});

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public void Update_ReturnsHttpNotFound_ForInvalidId()
        {
            // Arrange
            const int testSessionId = 123;
            var mockRepo = new Mock<ITodoRepository>();
            mockRepo.Setup(repo => repo.Find(testSessionId))
                .Returns((Todo) null);
            var controller = new TodoController(mockRepo.Object);

            // Act
            var result = controller.Update(testSessionId, new Todo {Name = "OK"});

            // Assert
            Assert.IsType<NotFoundResult>(result);
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