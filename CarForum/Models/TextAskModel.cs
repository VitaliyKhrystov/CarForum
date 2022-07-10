using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CarForum.Models
{
    public class TextAskModel
    {
        [Required]
        [Display(Name ="Short form of your question")]
        public string QuestionShort { get; set; }

        [Required]
        [Display(Name = "Extension form of your question")]
        public string QuestionExtension { get; set; }
    }
}
