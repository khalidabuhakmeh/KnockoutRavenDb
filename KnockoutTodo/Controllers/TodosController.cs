using System.Linq;
using System.Web.Mvc;
using System.Web.WebPages;
using KnockoutTodo.Models.Indexes;
using KnockoutTodo.Models.Objects;
using KnockoutTodo.Models.ViewModels;
using Raven.Client;

namespace KnockoutTodo.Controllers
{
    public class TodosController : Controller
    {
        protected IDocumentSession Db { get; set; }

        public ActionResult Index()
        {
            var model = new IndexViewModel();
            model.Items = Db.Query<Todo, Todos_All>()
                .Where(x => !x.IsCompleted)
                .ToList();

            if (model.Items.Any())
                model.Messages.Add(string.Format("You have {0} todos pending", model.Items.Count));

            return View(model);
        }

        public ActionResult Create(Todo todo)
        {
            if (ModelState.IsValid)
            {
                Db.Store(todo);
                return Json(new { ok = true, todo });
            }

            return Json(new { ok = false });
        }

        public ActionResult Update(Todo input)
        {
            if (input == null || input.Id.IsEmpty())
                return Json(new { ok = false });

            var todo = Db.Load<Todo>(input.Id);

            if (todo != null)
            {
                todo.IsCompleted = input.IsCompleted;
                todo.Text = input.Text;
            }

            return Json(new { ok = true });
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            Db = MvcApplication.DocumentStore.OpenSession();

            base.OnActionExecuting(filterContext);
        }

        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (filterContext.Exception == null || filterContext.ExceptionHandled)
            {
                if (Db != null)
                {
                    Db.SaveChanges();
                }
            }

            base.OnActionExecuted(filterContext);
        }
    }
}
