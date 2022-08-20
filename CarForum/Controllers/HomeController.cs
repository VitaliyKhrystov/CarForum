using CarForum.Domain;
using CarForum.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;

namespace CarForum.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private DataManager dataManager;
        private readonly UserManager<User> userManager;

        public HomeController(ILogger<HomeController> logger, DataManager dataManager, UserManager<User> userManager)
        {
            _logger = logger;
            this.dataManager = dataManager;
            this.userManager = userManager;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var topics = dataManager.EFTopicFields.GetTopic().ToList();
            var responses = dataManager.EFResponses.GetResponse().ToList();
            var users = userManager.Users.ToList();

            TopicResponseUserModel model = new TopicResponseUserModel()
            {
                Topics = topics,
                Responses = responses,
                Users = users
            };

            return View(model);
        }

    }
}
