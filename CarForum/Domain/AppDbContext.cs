using CarForum.Domain.Entities;
using CarForum.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CarForum.Domain
{
    public class AppDbContext: IdentityDbContext<User>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<TopicField> TopicFields { get; set; }
        public DbSet<Response> Responses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<TopicField>()
                        .HasMany(r => r.Responces)
                        .WithOne(t => t.TopicField)
                        .HasForeignKey(k => k.TopicFieldID)
                        .HasPrincipalKey(k => k.Id);
                      
            modelBuilder.Entity<User>()
                        .HasMany(t => t.TopicFields)
                        .WithOne(u => u.User)
                        .HasForeignKey(kf => kf.UserId)
                        .HasPrincipalKey(kp => kp.Id);

            modelBuilder.Entity<User>()
                       .HasMany(t => t.Responses)
                       .WithOne(u => u.User)
                       .HasForeignKey(kf => kf.UserId)
                       .HasPrincipalKey(kp => kp.Id);

        }
    }
}
