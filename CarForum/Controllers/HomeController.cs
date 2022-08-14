using CarForum.Domain;
using CarForum.Domain.Entities;
using CarForum.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CarForum.Controllers
{
    [Authorize(Roles = "Admin, User")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext context;
        private TopicResponseModel topicResponseModel;
        private DataManager dataManager;
        private TopicField topicField;

        public HomeController(ILogger<HomeController> logger, AppDbContext context, DataManager dataManager, TopicResponseModel topicResponseModel, TopicField topicField)
        {
            _logger = logger;
            this.context = context;
            this.dataManager = dataManager;
            this.topicResponseModel = topicResponseModel;
            this.topicField = topicField;
        }


        [AllowAnonymous]
        public IActionResult Index()
        {
            var topic = dataManager.EFTopicFields.GetTopic().ToList();

            return View(topic);
        }

        [HttpPost]
        public async Task<ActionResult> Create(TopicField topicField)
        {

            if (ModelState.IsValid)
            {
                await dataManager.EFTopicFields.CreateTopicAsync(topicField);
                await dataManager.EFTopicFields.SaveTopicAsync();
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> EditAsync(int id)
        {
            topicField = await dataManager.EFTopicFields.GetTopicByIdAsync(id);

            if (topicField != null)
            {
                return View(topicField);
            }

            return Redirect("Index");
        }

        [HttpPost]
        public async Task<ActionResult> EditPostAsync(TopicField _topicField)
        {
            dataManager.EFTopicFields.UpdateTopic(_topicField);
            await dataManager.EFTopicFields.SaveTopicAsync();

            return Redirect("Index");
        }

        public async Task<ActionResult> Delete(int id)
        {
           
            foreach (var item in context.Responses.ToList())
            {
                if (item.TopicFieldID == id)
                {
                    dataManager.EFResponses.DeleteResponse(item);
                    await dataManager.EFResponses.SaveResponseAsync();
                }
            }

            topicField = await dataManager.EFTopicFields.GetTopicByIdAsync(id);
            dataManager.EFTopicFields.DeleteTopic(topicField);
            await dataManager.EFTopicFields.SaveTopicAsync();

            return RedirectToAction("Index");

        }

    }
}
