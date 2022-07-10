using CarForum.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarForum.Domain.Repositories.Abstract
{
    public interface ITopicFieldRepository
    {
        IQueryable<TopicField> GetTopic();
        void CreateTopic (TopicField entity);
        void UpdateTopic (TopicField entity);
        TopicField GetTopicById(int id);
        void SaveTopic();
        void DeleteTopic(int id);
    }
}
