using CarForum.Domain.Entities;
using System.Linq;
using System.Threading.Tasks;

namespace CarForum.Domain.Repositories.Abstract
{
    public interface IResponseRepository
    {
        IQueryable<Response> GetResponse();
        Task CreateResponseAsync(Response entity);
        void UpdateResponse(Response entity);
        Task<Response> GetResponseByIdAsync(int id);
        Task SaveResponseAsync();
        void DeleteResponse(Response response);
    }
}
