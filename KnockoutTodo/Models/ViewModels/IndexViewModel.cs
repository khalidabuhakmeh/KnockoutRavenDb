using System.Collections.Generic;
using System.Web.Mvc;
using KnockoutTodo.Models.Objects;
using Newtonsoft.Json;

namespace KnockoutTodo.Models.ViewModels
{
    public class IndexViewModel
    {
        public IndexViewModel()
        {
            Items = new List<Todo>();
            Messages = new List<string>();
        }

        public IList<Todo> Items { get; set; }
        public IList<string> Messages { get; set; }

        public string ToJson(UrlHelper url)
        {
            return JsonConvert.SerializeObject(new
            {
                items = Items,
                messages = Messages,
                create = url.Action("create", "todos"),
                update = url.Action("update", "todos")
            });
        }
    }
}