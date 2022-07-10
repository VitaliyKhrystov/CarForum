using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarForum.Domain.Entities
{
    public class Responce
    {
        public int id { get; set; }
        public string Reply { get; set; }
        public int TopicFieldID { get; set; }
        public TopicField TopicField { get; set; }
    }
}
