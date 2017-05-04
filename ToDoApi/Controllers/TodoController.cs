using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using ToDoApi.Models;
using ToDoApi.Repository;

namespace ToDoApi.Controllers
{
    [Route("todos")]
    public class TodoController : Controller
    {
        private readonly ITodoRepository _todoRepository;

        public TodoController(ITodoRepository todoRepository)
        {
            _todoRepository = todoRepository;
        }

        [HttpGet]
        public IEnumerable<Todo> GetAll()
        {
            return _todoRepository.GetAll();
        }

        [HttpGet("{id}", Name = "GetTodo")]
        public IActionResult GetById(long id)
        {
            var item = _todoRepository.Find(id);
            if (item == null)
            {
                return NotFound();
            }
            return new ObjectResult(item);
        }

        [HttpPost]
        public IActionResult Create([FromBody] Todo todo)
        {
            if (todo == null || !todo.CheckValidName())
            {
                return BadRequest();
            }

            if (todo.TodoItems == null)
            {
                todo.TodoItems = new List<TodoItem>();
            }
            _todoRepository.Create(todo);

            return CreatedAtRoute("GetTodo", new {id = todo.Id}, todo);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            var todo = _todoRepository.Find(id);
            if (todo == null)
            {
                return NotFound();
            }

            _todoRepository.Remove(id);
            return new NoContentResult();
        }

        [HttpPut("{id}")]
        public IActionResult Update(long id, [FromBody] Todo payload)
        {
            if (payload == null || !payload.CheckValidName())
            {
                return BadRequest();
            }

            var todo = _todoRepository.Find(id);
            if (todo == null)
            {
                return NotFound();
            }

            todo.Name = payload.Name;

            _todoRepository.Update(todo);
            return CreatedAtRoute("GetTodo", new {id = todo.Id}, todo);
        }

        [HttpGet("items/{id}", Name = "GetTodoItem")]
        public IActionResult GetItemById(long id)
        {
            var item = _todoRepository.FindItem(id);
            if (item == null)
            {
                return NotFound();
            }
            return new ObjectResult(item);
        }

        [HttpPost("{todoId}")]
        public IActionResult AddItem(long todoId, [FromBody] TodoItem item)
        {
            if (item == null || !item.CheckValidName())
            {
                return BadRequest();
            }

            var todo = _todoRepository.Find(todoId);
            if (todo == null)
            {
                return NotFound();
            }

            item.TodoId = todoId;

            _todoRepository.AddItem(todoId, item);

            return CreatedAtRoute("GetTodoItem", new {id = item.Id}, item);
        }

        [HttpDelete("items/{id}")]
        public IActionResult DeleteItem(long id)
        {
            var todo = _todoRepository.FindItem(id);
            if (todo == null)
            {
                return NotFound();
            }

            _todoRepository.RemoveItem(id);
            return new NoContentResult();
        }

        [HttpPut("items/{id}")]
        public IActionResult UpdateItem(long id, [FromBody] TodoItem payload)
        {
            if (payload == null || !payload.CheckValidName())
            {
                return BadRequest();
            }

            var item = _todoRepository.FindItem(id);
            if (item == null)
            {
                return NotFound();
            }

            item.IsComplete = payload.IsComplete;
            item.Name = payload.Name;

            _todoRepository.UpdateItem(item);
            return CreatedAtRoute("GetTodoItem", new {id = item.Id}, item);
        }
    }
}