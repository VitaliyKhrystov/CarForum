using CarForum.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarForum.Domain
{
    public class AppDbContext: DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<TopicField> TopicFields { get; set; }
        public DbSet<Response> Responses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TopicField>()
                        .HasMany(r => r.Responces)
                        .WithOne(t => t.TopicField)
                        .HasForeignKey(k => k.TopicFieldID)
                        .HasPrincipalKey(k => k.Id);
        }
    }
}
