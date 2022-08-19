using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CarForum.ViewModel
{
    public class CreateTopicViewModel
    {
        [Required(ErrorMessage = "Topic not specified!")]
        public string TopicShort { get; set; }

        [Required(ErrorMessage = "Extension topic not specified!")]
        public string TopicExtension { get; set; }

        public IFormFile UploadFile { get; set; }
    }
}
