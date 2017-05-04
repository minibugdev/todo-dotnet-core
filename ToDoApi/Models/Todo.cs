using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;

namespace ToDoApi.Models
{
    public class Todo
    {
        private const int NameLimit = 10;
        private const string NameValidatePattern = @"^[a-zA-Z0-9\ ]+$";

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        public string Name { get; set; }
        public List<TodoItem> TodoItems { get; set; }

        public bool CheckValidName()
        {
            return !string.IsNullOrWhiteSpace(Name)
                   && Name.Length <= NameLimit
                   && new Regex(NameValidatePattern).IsMatch(Name);
        }
    }
}