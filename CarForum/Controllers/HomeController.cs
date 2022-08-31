using CarForum.Domain;
using CarForum.Domain.Entities;
using CarForum.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
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
        private readonly AppDbContext appDbContext;

        [BindProperty(SupportsGet = true)]
        public string SearchTopic { get; set; }

        public HomeController(ILogger<HomeController> logger, DataManager dataManager, UserManager<User> userManager, AppDbContext appDbContext)
        {
            _logger = logger;
            this.dataManager = dataManager;
            this.userManager = userManager;
            this.appDbContext = appDbContext;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var topics = dataManager.EFTopicFields.Search(SearchTopic).ToList();
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
