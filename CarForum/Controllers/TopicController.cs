using CarForum.Domain;
using CarForum.Domain.Entities;
using CarForum.Domain.Repositories.Abstract;
using CarForum.Domain.Repositories.EntityFrameWork;
using CarForum.Models;
using Microsoft.AspNetCore.Authorization;
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
    [Authorize(Roles = "Admin, User")]
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

        public async Task<ActionResult> ReplyAsync(int id)
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
        public async Task<ActionResult> ReplyPostAsync(int id, string reply)
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

            int ID = topicField.Id;

            return RedirectToAction("Reply", "Topic", new { id = ID });
        }

        [HttpGet]
        public async Task<ActionResult> EditAsync(int id)
        {
            response = await dataManager.EFResponses.GetResponseByIdAsync(id);
            topicResponseModel.Responces = new List<Response>() { response };

            foreach (var item in context.TopicFields.ToList())
            {
                if (item.Id == response.TopicFieldID)
                {
                    topicResponseModel.TopicField = item;
                }
            }

            return View(topicResponseModel);
        }

        [HttpPost]
        public async Task<ActionResult> EditPostAsync(Response _response)
        {

            dataManager.EFResponses.UpdateResponse(_response);
            await dataManager.EFResponses.SaveResponseAsync();

            int ID = _response.TopicFieldID;

            return RedirectToAction("Reply", "Topic", new { id = ID });
        }

        [HttpPost]
        public async Task<ActionResult> DeleteAsync(int id)
        {
            response = await dataManager.EFResponses.GetResponseByIdAsync(id);
            dataManager.EFResponses.DeleteResponse(response);
            await dataManager.EFResponses.SaveResponseAsync();

            int ID = response.TopicFieldID;


            return RedirectToAction("Reply", "Topic", new { id = ID });

        }
    }
}
