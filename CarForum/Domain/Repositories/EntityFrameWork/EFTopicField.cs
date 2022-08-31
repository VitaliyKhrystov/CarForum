using CarForum.Domain.Entities;
using CarForum.Domain.Repositories.Abstract;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarForum.Domain.Repositories.EntityFrameWork
{
    public class EFTopicField: ITopicFieldRepository
    {
        private readonly AppDbContext context;
        public EFTopicField(AppDbContext context)
        {
            this.context = context;
        }
        public IQueryable<TopicField> GetTopic() 
        {
            return context.TopicFields;
        }

        public async Task CreateTopicAsync(TopicField entity)
        {
            await context.TopicFields.AddAsync(entity);
        }

        public async Task<TopicField> GetTopicByIdAsync(int id)
        {
            return await context.TopicFields.FirstOrDefaultAsync(x => x.Id == id);
        }

        public void UpdateTopic(TopicField entity)
        {
           context.Entry(entity).State = EntityState.Modified;
        }

        public async Task SaveTopicAsync()
        {
           await context.SaveChangesAsync();
        }

        public void DeleteTopic(TopicField entity)
        {
             if (entity != null)
            {
                context.TopicFields.Remove(entity);
            }
        }

        public IQueryable<TopicField> Search(string searchTopic)
        {
            if (String.IsNullOrEmpty(searchTopic))
            {
                return GetTopic();
            }

            return GetTopic().Where(t => t.QuestionShort.Contains(searchTopic) ||
                                         t.QuestionExtension.Contains(searchTopic));
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
