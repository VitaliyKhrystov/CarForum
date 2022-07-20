using CarForum.Domain.Entities;
using System.Linq;


namespace CarForum.Domain.Repositories.Abstract
{
    public interface IResponseRepository
    {
        IQueryable<Response> GetResponse();
        void CreateResponse(Response entity);
        void UpdateResponse(Response entity);
        Response GetResponseById(int id);
        void SaveResponse();
        void DeleteResponse(int id);
    }
}
