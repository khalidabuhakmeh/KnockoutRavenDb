using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace KnockoutTodo.Models.Objects
{
    public class Todo
    {
        public string Id { get; set; }
        [Required]
        public string Text { get; set; }
        public bool IsCompleted { get; set; }
    }
}