using CarForum.Domain.Entities;
using CarForum.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarForum.ViewModel
{
    public class TopicResponsesUsersViewModel
    {
        public TopicField Topic { get; set; }
        public ICollection<Response> Responses { get; set; }
        public ICollection<User> Users { get; set; }

        public TopicResponsesUsersViewModel()
        {
            Responses = new List<Response>();
            Users = new List<User>();
        }
    }
}
