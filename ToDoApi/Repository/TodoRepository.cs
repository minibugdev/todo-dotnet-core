using System.Collections.Generic;
using System.Linq;
using ToDoApi.Dao;
using ToDoApi.Models;

namespace ToDoApi.Repository
{
    public class TodoRepository : ITodoRepository
    {
        private readonly TodoContext _context;

        public TodoRepository(TodoContext context)
        {
            _context = context;
        }

        #region Todo

        public IEnumerable<Todo> GetAll()
        {
            return _context.Todos.ToList();
        }

        public Todo Find(long id)
        {
            return _context.Todos.FirstOrDefault(t => t.Id == id);
        }

        public void Create(Todo todo)
        {
            _context.Todos.Add(todo);
            _context.SaveChanges();
        }

        public void Remove(long id)
        {
            var entity = _context.Todos.First(t => t.Id == id);
            _context.Todos.Remove(entity);
            _context.SaveChanges();
        }

        public void Update(Todo todo)
        {
            _context.Todos.Update(todo);
            _context.SaveChanges();
        }

        #endregion

        #region Todo Item

        public TodoItem FindItem(long id)
        {
            return _context.TodoItems.FirstOrDefault(t => t.Id == id);
        }

        public void AddItem(long todoId, TodoItem item)
        {
            var todo = Find(todoId);
            todo.TodoItems.Add(item);
            _context.SaveChanges();
        }

        public void RemoveItem(long id)
        {
            var entity = _context.TodoItems.First(t => t.Id == id);
            _context.TodoItems.Remove(entity);
            _context.SaveChanges();
        }

        public void UpdateItem(TodoItem item)
        {
            _context.TodoItems.Update(item);
            _context.SaveChanges();
        }

        #endregion
    }
}