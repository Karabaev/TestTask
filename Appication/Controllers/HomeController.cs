namespace Appication.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Appication.Models;
    using Data.Repository;

    public class HomeController : Controller
    {
        public HomeController(  IContactInfoRepository contactInfoRepo, 
                                IOrganizationRepository organizationRepo, 
                                IPersonRepository personRepo, 
                                IPositionRepository positionRepo)
        {
            this.contactInfoRepository = contactInfoRepo;
            this.organizationRepository = organizationRepo;
            this.personRepository = personRepo;
            this.positionRepository = positionRepo;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        private readonly IContactInfoRepository contactInfoRepository;
        private readonly IOrganizationRepository organizationRepository;
        private readonly IPersonRepository personRepository;
        private readonly IPositionRepository positionRepository;
    }
}
