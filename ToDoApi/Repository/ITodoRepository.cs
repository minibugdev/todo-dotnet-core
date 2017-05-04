using System.Collections.Generic;
using ToDoApi.Models;

namespace ToDoApi.Repository
{
    public interface ITodoRepository
    {
        IEnumerable<Todo> GetAll();
        Todo Find(long id);
        void Create(Todo todo);
        void Remove(long id);
        void Update(Todo todo);

        TodoItem FindItem(long id);
        void AddItem(long todoId, TodoItem item);
        void RemoveItem(long id);
        void UpdateItem(TodoItem item);
    }
}