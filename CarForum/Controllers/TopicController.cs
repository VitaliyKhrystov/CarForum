using CarForum.Domain;
using CarForum.Domain.Entities;
using CarForum.Domain.Repositories.Abstract;
using CarForum.Models;
using CarForum.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
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
        private readonly UserManager<User> userManager;
        private readonly IWebHostEnvironment env;

        public TopicController(ILogger<HomeController> logger, AppDbContext context, DataManager dataManager, TopicResponseModel topicResponseModel, TopicField  topicField, Response response, UserManager<User> userManager, IWebHostEnvironment env)
        {
            _logger = logger;
            this.context = context;
            this.dataManager = dataManager;
            this.topicResponseModel = topicResponseModel;
            this.topicField = topicField;
            this.response = response;
            this.userManager = userManager;
            this.env = env;
            responses = new List<Response>();
        }

        [HttpGet]
        public ActionResult CreateTopic()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> CreateTopic(CreateTopicViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await userManager.FindByNameAsync(User.Identity.Name);

                topicField = new TopicField()
                {
                    QuestionShort = model.TopicShort,
                    QuestionExtension = model.TopicExtension,
                    User = user,
                    TopicData = DateTime.Now
                };

                if (model.UploadFile != null)
                {
                    string fileName = Guid.NewGuid().ToString() + "-" + model.UploadFile.FileName;
                    string path = Path.Combine(env.WebRootPath, "img/UserFiles", fileName);
                    using (var filestrem = new FileStream(path, FileMode.Create))
                    {
                        await model.UploadFile.CopyToAsync(filestrem);
                    }

                    topicField.ImageName = fileName;
                }

                await dataManager.EFTopicFields.CreateTopicAsync(topicField);
                await dataManager.EFTopicFields.SaveTopicAsync(); 
            }

            return RedirectToAction("Index", "Home");
        }


            [HttpGet]
        public async Task<IActionResult> EditTopic(int id)
        {
            topicField = await dataManager.EFTopicFields.GetTopicByIdAsync(id);

                  
            if (topicField != null)
            {
                var model = new CreateTopicViewModel()
                {
                    TopicShort = topicField.QuestionShort,
                    TopicExtension = topicField.QuestionExtension
                };

                ViewBag.ImageName = topicField.ImageName;
                ViewBag.TopicId = topicField.Id;

                return View(model);
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<ActionResult> EditTopic(CreateTopicViewModel model, int topicId, bool isSelected)
        {

            if (ModelState.IsValid)
            {
                User user = await userManager.FindByNameAsync(User.Identity.Name);
                topicField = await dataManager.EFTopicFields.GetTopicByIdAsync(topicId);

                if (isSelected || (topicField.ImageName != null && model.UploadFile != null))
                {
                    string path = Path.Combine(env.WebRootPath, "img/UserFiles", topicField.ImageName);
                    System.IO.File.Delete(path);
                    topicField.ImageName = null;
                    dataManager.EFTopicFields.UpdateTopic(topicField);
                    await dataManager.EFTopicFields.SaveTopicAsync();
                }

                if (model.UploadFile != null)
                {
                    string fileName = Guid.NewGuid().ToString() + "-" + model.UploadFile.FileName;
                    string path = Path.Combine(env.WebRootPath, "img/UserFiles", fileName);
                    using (var filestrem = new FileStream(path, FileMode.Create))
                    {
                        await model.UploadFile.CopyToAsync(filestrem);
                    }

                    topicField.ImageName = fileName;
                }

                if ( (topicField.QuestionShort != model.TopicShort) || (topicField.QuestionExtension != model.TopicExtension) || (model.UploadFile != null) )
                {
                    topicField.QuestionShort = model.TopicShort;
                    topicField.QuestionExtension = model.TopicExtension;
                    topicField.User = user;
                    topicField.TopicData = DateTime.Now;
                
                    dataManager.EFTopicFields.UpdateTopic(topicField);
                    await dataManager.EFTopicFields.SaveTopicAsync();
                } 
            }

            return RedirectToAction("Index", "Home");
        }

        public async Task<ActionResult> DeleteTopic(int id)
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

            if (topicField.ImageName != null)
            {
                string path = Path.Combine(env.WebRootPath, "img/UserFiles", topicField.ImageName);
                System.IO.File.Delete(path);
            }
              
            dataManager.EFTopicFields.DeleteTopic(topicField);
            await dataManager.EFTopicFields.SaveTopicAsync();

            return RedirectToAction("Index", "Home");
        }


        [HttpGet]
        public async Task<ActionResult> CreateReply(int id)
        {
            topicField = await dataManager.EFTopicFields.GetTopicByIdAsync(id);

            foreach (var reply in dataManager.EFResponses.GetResponse().ToList())
            {
                if (reply.TopicFieldID == id)
                {
                    responses.Add(reply);
                }
            }

            var model = new TopicResponsesUsersViewModel()
            {
                Topic = topicField,
                Responses = responses,
                Users = userManager.Users.ToList()
            };

            return View(model);

        }

        [HttpPost]
        public async Task<ActionResult> CreateReply(int id, string reply)
        {
            topicField = await dataManager.EFTopicFields.GetTopicByIdAsync(id);
            User user = await userManager.FindByNameAsync(User.Identity.Name);

            if (reply != null || reply == string.Empty)
            {
                response = new Response() { Reply = reply, TopicField = topicField, ReplyData = DateTime.Now, User = user };

               await dataManager.EFResponses.CreateResponseAsync(response);
               await dataManager.EFResponses.SaveResponseAsync();
            }
            else
            {
                return Redirect("/Home/Index");
            }

            int ID = topicField.Id;

            return RedirectToAction("CreateReply", "Topic", new { id = ID });
        }

        [HttpGet]
        public async Task<ActionResult> EditReply(int id)
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
        public async Task<ActionResult> EditReply(Response _response)
        {
            User user = await userManager.FindByNameAsync(User.Identity.Name);
            _response.UserId = user.Id;
            _response.ReplyData = DateTime.Now;
            dataManager.EFResponses.UpdateResponse(_response);
            await dataManager.EFResponses.SaveResponseAsync();

            int ID = _response.TopicFieldID;

            return RedirectToAction("CreateReply", "Topic", new { id = ID });
        }

        [HttpPost]
        public async Task<ActionResult> DeleteReply(int id)
        {
            response = await dataManager.EFResponses.GetResponseByIdAsync(id);
            dataManager.EFResponses.DeleteResponse(response);
            await dataManager.EFResponses.SaveResponseAsync();

            int ID = response.TopicFieldID;


            return RedirectToAction("CreateReply", "Topic", new { id = ID });

        }
    }
}
