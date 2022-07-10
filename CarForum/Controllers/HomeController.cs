using CarForum.Domain;
using CarForum.Domain.Entities;
using CarForum.Domain.Repositories.EntityFrameWork;
using CarForum.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace CarForum.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext context;

        public HomeController(ILogger<HomeController> logger, AppDbContext context)
        {
            _logger = logger;
            this.context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(TopicField topicField)
        {
            EFTopicField fTopicField = new EFTopicField(context);
           if (ModelState.IsValid)
            {
                fTopicField.CreateTopic(topicField);
                fTopicField.SaveTopic();

                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

    }
}
