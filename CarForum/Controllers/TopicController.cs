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
        private TopicField topicField { get; set; }
        private Response response { get; set; }

        public TopicController(ILogger<HomeController> logger, AppDbContext context, DataManager dataManager, TopicResponseModel topicResponseModel)
        {
            _logger = logger;
            this.context = context;
            this.dataManager = dataManager;
            this.topicResponseModel = topicResponseModel;
            responses = new List<Response>();
        }
        public ActionResult Add()
        {
            return View();
        }

        public ActionResult Page(int id)
        {
            topicField = dataManager.EFTopicFields.GetTopicById(id);

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
        public ActionResult Reply(int id, string reply)
        {
            topicField = dataManager.EFTopicFields.GetTopicById(id);

            if (reply != null || reply == string.Empty)
            {
                response = new Response() { Reply = reply, TopicField = topicField };
                dataManager.EFResponses.CreateResponse(response);
                dataManager.EFResponses.SaveResponse();
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
