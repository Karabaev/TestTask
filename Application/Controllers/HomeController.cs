namespace Application.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Application.Models;
    using Data.Repository;

    public class HomeController : Controller
    {
        public HomeController(  IContactInfoRepository contactInfoRepository,
                                IOrganizationRepository organizationRepository,
                                IPersonRepository personRepository, 
                                IPositionRepository positionRepository)
        {
            this.contactInfoRepository = contactInfoRepository;
            this.organizationRepository = organizationRepository;
            this.personRepository = personRepository;
            this.positionRepository = positionRepository;
        }

        public IActionResult Index()
        {
            ICollection<IndexPersonViewModel> models = new List<IndexPersonViewModel>();

            foreach (var item in this.personRepository.GetAll())
            {
                models.Add(new IndexPersonViewModel(item));
            }

            ViewBag.Title = "Список контактов";
            return View(models);
        }

        private readonly IContactInfoRepository contactInfoRepository;
        private readonly IOrganizationRepository organizationRepository;
        private readonly IPersonRepository personRepository;
        private readonly IPositionRepository positionRepository;
    }
}
