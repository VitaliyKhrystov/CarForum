using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CarForum.Domain.Entities
{
    public class Response
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Response not specified!")]
        public string Reply { get; set; }
        public int? TopicFieldID { get; set; }
        public TopicField TopicField { get; set; }
    }
}
