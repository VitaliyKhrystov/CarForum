using CarForum.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace CarForum.Models
{
    public class User: IdentityUser
    {
        public DateTime Date { get; set; }
        public ICollection<TopicField> TopicFields { get; set; }
        public ICollection<Response> Responses { get; set; }

        public User()
        {
            TopicFields = new List<TopicField>();
            Responses = new List<Response>();
        }
    }
}
