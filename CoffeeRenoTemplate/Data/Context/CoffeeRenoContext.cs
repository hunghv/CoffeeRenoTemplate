using Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Data.Context
{
    public class CoffeeRenoContext : DbContext
    {
        public CoffeeRenoContext(DbContextOptions<CoffeeRenoContext> options)
            : base(options)
        { }

        public DbSet<AdsType> AdsTypes { get; set; }
        public DbSet<AdsForm> AdsForms { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<PostType> PostTypes { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<PostImage> PostImages { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AdsType>().ToTable("AdsType");
            modelBuilder.Entity<AdsForm>().ToTable("AdsForm");
            modelBuilder.Entity<Image>().ToTable("Image");
            modelBuilder.Entity<PostType>().ToTable("PostType");
            modelBuilder.Entity<Post>().ToTable("Post");
            modelBuilder.Entity<PostImage>().ToTable("PostImage");
        }
    }
}
