using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarForum.Domain.Entities
{
    public class TopicField
    {
        public int Id { get; set; }
        public string QuestionShort { get; set; }
        public string QuestionExtension { get; set; }
        public List<Responce> Responces { get; set; } = new List<Responce>();

    }
}

