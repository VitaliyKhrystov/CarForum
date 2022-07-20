using CarForum.Domain;
using CarForum.Domain.Entities;
using CarForum.Domain.Repositories.Abstract;
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
        private TopicResponseModel topicResponseModel;
        private DataManager dataManager;

        public HomeController(ILogger<HomeController> logger, AppDbContext context, DataManager dataManager, TopicResponseModel topicResponseModel)
        {
            _logger = logger;
            this.context = context;
            this.dataManager = dataManager;
            this.topicResponseModel = topicResponseModel;
        }

        public IActionResult Index()
        {
            var topic = new List<TopicField>();
            topic = dataManager.EFTopicFields.GetTopic().ToList();
           
            return View(topic);
        }

        [HttpPost]
        public async Task<ActionResult>  Create(TopicField topicField)
        {

           if (ModelState.IsValid)
            {
               await dataManager.EFTopicFields.CreateTopicAsync(topicField);
               await dataManager.EFTopicFields.SaveTopic();
            }

            return RedirectToAction("Index");
        }

    }
}
