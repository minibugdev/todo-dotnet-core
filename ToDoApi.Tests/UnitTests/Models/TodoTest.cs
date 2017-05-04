using ToDoApi.Models;
using Xunit;

namespace ToDoApi.Tests.UnitTests.Models
{
    public class TodoTest
    {
        private readonly Todo _todo;

        public TodoTest()
        {
            _todo = new Todo();
        }

        [Fact]
        public void ReturnFalseGivenNameNull()
        {
            var result = _todo.CheckValidName();
            Assert.False(result, "Name should not be null");
        }

        [Fact]
        public void ReturnFalseGivenNameEmpty()
        {
            _todo.Name = "";
            var result = _todo.CheckValidName();
            Assert.False(result, "Name should not be empty");
        }

        [Fact]
        public void ReturnFalseGivenNameWhitSpace()
        {
            _todo.Name = "    ";
            var result = _todo.CheckValidName();
            Assert.False(result, "Name should not be whit space");
        }

        [Fact]
        public void ReturnFalseGivenNameLengthMoreThan10()
        {
            _todo.Name = ".Net Core Unit Test";
            var result = _todo.CheckValidName();
            Assert.False(result, "Name should not be more than 10");
        }

        [Fact]
        public void ReturnFalseGivenNameNotEn()
        {
            _todo.Name = ".เนต คอ";
            var result = _todo.CheckValidName();
            Assert.False(result, "Name should be English");
        }

        [Fact]
        public void ReturnTrueGivenNameEnAndLessThan10()
        {
            _todo.Name = "Teera Nai";
            var result = _todo.CheckValidName();
            Assert.True(result, "Name should be English and less than 10");
        }
    }
}