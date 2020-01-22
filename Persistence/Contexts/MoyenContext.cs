using Microsoft.EntityFrameworkCore;
using Moyen.Domain.Models;

namespace Moyen.Persistence.Contexts
{
    public class MoyenContext : DbContext
    {
        
        public DbSet<Article> Articles { get; set; }
        public DbSet<Person> Persons { get; set; }
        public DbSet<ArticleFave> ArticleFaves { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<ArticleTag> ArticleTags { get; set; }
        public DbSet<Comment> Comments { get; set; }


        public MoyenContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<ArticleTag>(a =>
            {
                a.HasKey(r => new { r.ArticleId, r.TagId });

                a.HasOne(t => t.Article)
                    .WithMany(i => i.ArticleTags)
                    .HasForeignKey(c => c.ArticleId);
            });

            builder.Entity<ArticleFave>(a =>
            {
                a.HasKey(r => new { r.ArticleId, r.PersonId });

                a.HasOne(t => t.Article)
                    .WithMany(i => i.ArticleFaves)
                    .HasForeignKey(c => c.ArticleId);
            });
        }

    }

}