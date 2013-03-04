using System.Linq;
using KnockoutTodo.Models.Objects;
using Raven.Client.Indexes;

namespace KnockoutTodo.Models.Indexes
{
    public class Todos_All : AbstractIndexCreationTask<Todo, Todos_All.Result>
    {
        public class Result
        {
            public string Id { get; set; }
            public bool IsCompleted { get; set; }
        }

        public Todos_All()
        {
            Map = todos => from todo in todos
                           select new Result
                           {
                               IsCompleted = todo.IsCompleted,
                               Id = todo.Id
                           };
        }
    }
}