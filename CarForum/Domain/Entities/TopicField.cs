using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CarForum.Domain.Entities
{
    public class TopicField
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Topic not specified!")]
        public string QuestionShort { get; set; }

        [Required(ErrorMessage = "Extension topic not specified!")]
        public string QuestionExtension { get; set; }
        [Required(ErrorMessage = "Response not specified!")]
        public ICollection<Response> Responces { get; set; }
        public TopicField()
        {
            Responces = new List<Response>();
        }

    }
}

