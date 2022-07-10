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

        public void CreateTopic(TopicField entity)
        {
            context.TopicFields.Add(entity);
        }

        public TopicField GetTopicById(int id)
        {
            return context.TopicFields.FirstOrDefault(x => x.Id == id);
        }

        public void UpdateTopic(TopicField entity)
        {
            context.Entry(entity).State = EntityState.Modified;
        }

        public void SaveTopic()
        {
            context.SaveChanges();
        }

        public void DeleteTopic(int id)
        {
            TopicField topicField = context.TopicFields.Find(id);

            if (topicField != null)
            {
                context.TopicFields.Remove(topicField);
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
