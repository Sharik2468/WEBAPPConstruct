using Microsoft.EntityFrameworkCore;

namespace WebAPI.Models
{
    public partial class BloggingContext : DbContext
    {
        protected readonly IConfiguration Configuration;

        public BloggingContext(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
        }

        //public BloggingContext(DbContextOptions<BloggingContext> options) : base(options)
        //{
        //}

        public virtual DbSet<Blog> Blog { get; set; }
        public virtual DbSet<Post> Post { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Blog>(entity =>
            {
                entity.Property(e => e.Url).IsRequired();
            });
            modelBuilder.Entity<Post>(entity =>
            {
                entity.HasOne(d => d.Blog)
                .WithMany(p => p.Post)
                .HasForeignKey(d => d.BlogId);
            });
        }
    }
}
