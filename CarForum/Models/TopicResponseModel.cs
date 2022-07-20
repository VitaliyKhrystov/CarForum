using CarForum.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarForum.Models
{
    public class TopicResponseModel
    {
        public TopicField TopicField { get; set; }
        public IEnumerable<Response>  Responces { get; set; }
    }
}
