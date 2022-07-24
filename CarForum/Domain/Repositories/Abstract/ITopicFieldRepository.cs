using CarForum.Domain.Entities;
using System.Linq;
using System.Threading.Tasks;

namespace CarForum.Domain.Repositories.Abstract
{
    public interface ITopicFieldRepository
    {
        IQueryable<TopicField> GetTopic();
        Task CreateTopicAsync (TopicField entity);
        void UpdateTopic (TopicField entity);
        Task<TopicField> GetTopicByIdAsync(int id);
        Task SaveTopicAsync();
        void DeleteTopic(TopicField entity);
    }
}
