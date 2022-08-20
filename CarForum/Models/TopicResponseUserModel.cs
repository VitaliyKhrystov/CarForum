using CarForum.Domain.Entities;
using System.Collections.Generic;

namespace CarForum.Models
{
    public class TopicResponseUserModel
    {
        public ICollection<TopicField> Topics { get; set; }
        public ICollection<Response> Responses { get; set; }
        public ICollection<User> Users { get; set; }

        public TopicResponseUserModel()
        {
            Topics = new List<TopicField>();
            Responses = new List<Response>();
            Users = new List<User>();
        }
    }
}
