using CarForum.Domain;
using CarForum.Domain.Entities;
using CarForum.Domain.Repositories.Abstract;
using CarForum.Domain.Repositories.EntityFrameWork;
using CarForum.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarForum.Controllers
{
    public class TopicController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext context;
        private TopicResponseModel topicResponseModel;
        private DataManager dataManager;
        private List<Response> responses;
        private TopicField topicField;
        private Response response;

        public TopicController(ILogger<HomeController> logger, AppDbContext context, DataManager dataManager, TopicResponseModel topicResponseModel, TopicField  topicField, Response response)
        {
            _logger = logger;
            this.context = context;
            this.dataManager = dataManager;
            this.topicResponseModel = topicResponseModel;
            this.topicField = topicField;
            this.response = response;
            responses = new List<Response>();
        }
        public ActionResult Add()
        {
            return View();
        }

        public async Task<ActionResult>  Page(int id)
        {
            topicField = await dataManager.EFTopicFields.GetTopicByIdAsync(id);

            topicResponseModel.TopicField = topicField;

            foreach (var item in context.Responses)
            {
                if (item.TopicFieldID == id)
                {
                    responses.Add(item);
                }
            }

            topicResponseModel.Responces = responses;

            return View(topicResponseModel);
        }
        [HttpPost]
        public async Task<ActionResult> Reply(int id, string reply)
        {
            topicField = await dataManager.EFTopicFields.GetTopicByIdAsync(id);

            if (reply != null || reply == string.Empty)
            {
                response = new Response() { Reply = reply, TopicField = topicField };
               await dataManager.EFResponses.CreateResponseAsync(response);
               await dataManager.EFResponses.SaveResponseAsync();
            }
            else
            {
                return Redirect("/Home/Index");
            }

            topicResponseModel.TopicField = topicField;

           foreach (var item in context.Responses)
            {
                if (item.TopicFieldID == id)
                {
                    responses.Add(item);
                }
            }

            topicResponseModel.Responces = responses;

            return View(topicResponseModel);
        }
    }
}
