using CarForum.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace CarForum.Controllers
{
    [Authorize(Roles = "Admin, User")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private DataManager dataManager;

        public HomeController(ILogger<HomeController> logger, DataManager dataManager)
        {
            _logger = logger;
            this.dataManager = dataManager;
        }


        [AllowAnonymous]
        public IActionResult Index()
        {
            var topic = dataManager.EFTopicFields.GetTopic().ToList();

            return View(topic);
        }

    }
}
