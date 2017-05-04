using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ToDoApi.Models;
using Xunit;

namespace ToDoApi.Tests.IntegrationTests
{
    public class TodoControllerTests : IClassFixture<TestFixture<Startup>>
    {
        private readonly HttpClient _client;

        public TodoControllerTests(TestFixture<Startup> fixture)
        {
            _client = fixture.Client;
        }

        [Fact]
        public async Task IndexGet_ReturnsInitialListOfTodo()
        {
            // Arrange - get a session known to exist
            var testSession = Startup.GetTestTodo();

            // Act
            var response = await _client.GetAsync("/todos");

            // Assert
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            Assert.True(responseString.Contains(testSession.Name));
        }

        [Fact]
        public async Task CreatePost_ReturnsBadRequest_GivenNullBody()
        {
            // Arrange
            var newTodo = ConvertObjectToStringContent(null);

            // Act
            var response = await _client.PostAsync("/todos", newTodo);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task CreatePost_ReturnsBadRequest_GivenInvlidName()
        {
            // Arrange
            var body = ConvertObjectToStringContent(new Todo {Name = "ห้ามภาษาไทย"});

            // Act
            var response = await _client.PostAsync("/todos", body);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task CreatePost_ReturnsCreatedTodo_GivenCorrectInputs()
        {
            // Arrange
            var newTodo = new Todo {Name = "Test Todo", TodoItems = new List<TodoItem>()};
            newTodo.TodoItems.Add(new TodoItem {Name = "Item 1"});
            newTodo.TodoItems.Add(new TodoItem {Name = "Item 2"});
            var body = ConvertObjectToStringContent(newTodo);

            // Act
            var response = await _client.PostAsync("/todos", body);

            // Assert
            response.EnsureSuccessStatusCode();

            var returnedSession = await response.Content.ReadAsStringAsync();
            var credtedTodo = ConvertStringToObject<Todo>(returnedSession);
            Assert.Equal("Test Todo", credtedTodo.Name);
            Assert.Equal(2, credtedTodo.TodoItems.Count);
            Assert.True(credtedTodo.TodoItems.Any(i => i.TodoId == credtedTodo.Id));
        }

        private static StringContent ConvertObjectToStringContent(object obj)
        {
            return new StringContent(JsonConvert.SerializeObject(obj), Encoding.UTF8, "application/json");
        }

        private static T ConvertStringToObject<T>(string str)
        {
            return JsonConvert.DeserializeObject<T>(str);
        }
    }
}