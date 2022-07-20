using CarForum.Domain.Entities;
using System.Linq;

namespace CarForum.Domain.Repositories.Abstract
{
    public interface ITopicFieldRepository
    {
        IQueryable<TopicField> GetTopic();
        async void CreateTopicAsync (TopicField entity);
        async void UpdateTopicAsync (TopicField entity);
        async TopicField GetTopicByIdAsync(int id);
        async void SaveTopicAsync();
        async void DeleteTopicAsync(int id);
    }
}
