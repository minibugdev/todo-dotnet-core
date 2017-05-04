using ToDoApi.Models;
using Xunit;

namespace ToDoApi.Tests
{
    public class TodoTest
    {
        private readonly Todo _todo;

        public TodoTest()
        {
            _todo = new Todo();
        }

        [Fact]
        public void ReturnFalseGivenNameEmpty()
        {
        }
    }
}