namespace Application
{
    public enum SearchTypes
    {
        Name,
        Organization,
        Position,
        ContactInfo
    }
}

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

        [HttpGet("/get-person")]
        public IActionResult GetPerson(GetPersonViewModel model)
        {
            if (!ModelState.IsValid)
                return Json(new { error = "Не указан идентификатор записи" });

            Person person = this.dataManager.GetPerson(model.Id);

            if (person == null)
                return Json(new { error = "Контакт не найден" });

            UpdatePersonViewModel viewModel = new UpdatePersonViewModel(person);
            return Json(new { obj = viewModel });
        }

        [HttpPost("/create")]
        public async Task<IActionResult> CreatePersonAsync([FromBody]CreatePersonViewModel model)
        {
            if (!ModelState.IsValid)
                return Json(new { error = "На форме есть некорректные данные" });

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
            }
            else
            {
                 return Json(new { error = "Внутренняя ошибка сервера. Не удалось сохранить запись." }); ;
            }
        }

        [HttpDelete("/remove")]
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

        [HttpDelete("/remove-contact-info")]
        public async Task<IActionResult> RemoveContactInfo([FromBody]RemoveContactInfoViewModel model)
        {
            if (!ModelState.IsValid)
                return Json(new { error = "Не указан идентификатор записи" });

            bool result = await this.dataManager.RemoveContactInfoAsync(model.Id.Value);

            if (result)
                return Ok();
            else
                return Json(new { error = "Не удалось удалить запись" });
        }
        [HttpPatch("/update-person")]
        public async Task<IActionResult> UpdatePersonAsync([FromBody]UpdatePersonViewModel model)
        {
            if (!ModelState.IsValid)
                return Json(new { error = "На форме есть некорректные данные" });

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

            bool result = await this.dataManager.UpdatePersonAsync(model.GetDomain(organization.Id, position.Id));
           
            if (result)
                return Ok(new { redirectUrl = Url.Action("Index") });
            else
                return Json(new { error = "Не удалось обновить контакт" });
        }

        [HttpGet("/search")]
        public IActionResult SearchPerson(SearchPersonViewModel model)
        {
            if(!ModelState.IsValid)
                return Json(new { error = "Не введена строка поиска" });

            ICollection<IndexPersonViewModel> viewModels = new List<IndexPersonViewModel>();
            IEnumerable<Person> models = null;

            switch (model.SearchType)
            {
                case SearchTypes.Name:
                    models = this.dataManager.FindPersonByName(model.SearchString);
                    break;
                case SearchTypes.Organization:
                    models = this.dataManager.FindPersonByOrganization(model.SearchString);
                    break;
                case SearchTypes.Position:
                    models = this.dataManager.FindPersonByPosition(model.SearchString);
                    break;
                case SearchTypes.ContactInfo:
                    models = this.dataManager.FindPersonByContactInfo(model.SearchString);
                    break;
                default:
                    break;
            }

            foreach (var item in models)
            {
                viewModels.Add(new IndexPersonViewModel(item));
            }

            ViewBag.Title = "Результаты поиска";
            return View("Index", viewModels);
        }

        private readonly DataManager dataManager;
    }
}
