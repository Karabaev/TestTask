namespace Application.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Application.Models;
    using Data.Context;

    public class HomeController : Controller
    {
        public HomeController(Context context)
        {
            this.context = context;
        }

        public IActionResult Index()
        {
            ICollection<IndexPersonViewModel> models = new List<IndexPersonViewModel>();

            foreach (var item in this.context.People)
            {
                models.Add(new IndexPersonViewModel(item));
            }

            ViewBag.Title = "Список контактов";
            return View(models);
        }

        private readonly Context context;
    }
}
