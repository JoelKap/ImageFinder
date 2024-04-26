using Microsoft.EntityFrameworkCore;

namespace ImageFinder.Dal
{
    public class ImageDbContext : DbContext
    {
        public DbSet<Image> Images { get; set; }
        public ImageDbContext(DbContextOptions<ImageDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Image>().HasKey(i => i.Id);
            modelBuilder.Entity<Image>().HasData(
                new Image { Id = 1, Url = "https://api.dicebear.com/8.x/pixel-art/png?seed="},
                new Image { Id = 2, Url = "https://api.dicebear.com/8.x/pixel-art/png?seed="},
                new Image { Id = 3, Url = "https://api.dicebear.com/8.x/pixel-art/png?seed="},
                new Image { Id = 4, Url = "https://api.dicebear.com/8.x/pixel-art/png?seed="}
            );
        }

        public class Image
        {
            public int Id { get; set; }
            public string? Url { get; set; }
        }
    }
}
