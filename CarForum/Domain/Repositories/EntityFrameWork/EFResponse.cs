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

        public async Task CreateResponseAsync(Response entity)
        {
           await context.Responses.AddAsync(entity);
        }

        public async Task<Response> GetResponseByIdAsync(int id)
        {
            return await context.Responses.FirstOrDefaultAsync(x => x.Id == id);
        }

        public void UpdateResponse(Response entity)
        {
            context.Entry(entity).State = EntityState.Modified;
        }

        public async Task SaveResponseAsync()
        {
            await context.SaveChangesAsync();
        }

        public void DeleteResponse(Response response)
        {
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
