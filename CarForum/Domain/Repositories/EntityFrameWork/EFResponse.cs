using CarForum.Domain.Entities;
using CarForum.Domain.Repositories.Abstract;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarForum.Domain.Repositories.EntityFrameWork
{
    public class EFResponse: IResponseRepository
    {
        private readonly AppDbContext context;
        public EFResponse(AppDbContext context)
        {
            this.context = context;
        }
        public IQueryable<Response> GetResponse()
        {
            return context.Responses;
        }

        public void CreateResponse(Response entity)
        {
            context.Responses.Add(entity);
        }

        public Response GetResponseById(int id)
        {
            return context.Responses.FirstOrDefault(x => x.Id == id);
        }

        public void UpdateResponse(Response entity)
        {
            context.Entry(entity).State = EntityState.Modified;
        }

        public void SaveResponse()
        {
            context.SaveChanges();
        }

        public void DeleteResponse(int id)
        {
            Response response = context.Responses.Find(id);

            if (response != null)
            {
                context.Responses.Remove(response);
            }
        }

        private bool disposed = false;

        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
