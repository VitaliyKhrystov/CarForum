using CarForum.Domain.Repositories.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarForum.Domain
{
    public class DataManager
    {
        public IResponseRepository EFResponses { get; set; }
        public ITopicFieldRepository EFTopicFields { get; set; }
        public DataManager(IResponseRepository response, ITopicFieldRepository topicField)
        {
            EFResponses = response;
            EFTopicFields = topicField;
        }
    }
}
