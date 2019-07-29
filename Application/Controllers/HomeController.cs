namespace Application.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Application.Models;
    using Data;
    using Data.Entities;
    using Common;

    public class HomeController : Controller
    {
        public HomeController(Context context)
        {
            dataManager = new DataManager(context);
        }

        [HttpGet]
        public IActionResult Index()
        {
            ICollection<IndexPersonViewModel> models = new List<IndexPersonViewModel>();

            foreach (var item in this.dataManager.GetAllPeople())
            {
                models.Add(new IndexPersonViewModel(item));
            }

            ViewBag.Title = "Список контактов";
            return View(models);
        }

        [HttpPost("/Create")]
        public async Task<IActionResult> CreatePersonAsync([FromBody]CreatePersonViewModel model)
        {
            if (!ModelState.IsValid)
                return Json(new { error = "На форме есть некорретные данные" });

            Organization organization = this.dataManager.FindOrganizationByName(model.OrganizationName);

            if (organization == null)
            {
                organization = new Organization { Name = model.OrganizationName };
                await this.dataManager.AddOrganizationAsync(organization);
            }
                
            Position position = this.dataManager.FindPositionByName(model.PositionName);

            if (position == null)
            {
                position = new Position { Name = model.PositionName };
                await this.dataManager.AddPositionAsync(position);
            }
                
            Person person = model.GetDomain(organization.Id, position.Id);
            bool result = await this.dataManager.AddPersonAsync(person);

            if (result)
            {
                return Ok(new { redirectUrl = Url.Action("Index") });
                //return Json(new { redirectUrl = Url.Action("Index") });
                //return RedirectToAction("Index");
            }
            else
            {
                 return Json(new { error = "Внутренняя ошибка сервера. Не удалось сохранить запись." }); ;
            }
        }

        [HttpDelete("/Remove")]
        public async Task<IActionResult> RemovePerson([FromBody]RemovePersonViewModel model)
        {
            if(!ModelState.IsValid)
                return Json(new { error = "Не указан идентификатор записи" });

            bool result = await dataManager.RemovePersonAsync(model.Id.Value);

            if (result)
                return Json(new { redirectUrl = Url.Action("Index") });
            else
                return Json(new { error = "Не удалось удалить запись" });
        }

        public async Task<IActionResult> UpdatePersonAsync(UpdatePersonViewModel model)
        {
            if (!ModelState.IsValid)
                return StatusCode(404);

            Person person = model.GetDomain();
            bool result = await this.dataManager.UpdatePersonAsync(person);

            if (result)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return StatusCode(500);
            }
        }

        private readonly DataManager dataManager;
    }
}
