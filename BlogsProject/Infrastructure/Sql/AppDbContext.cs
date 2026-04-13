using BlogsProject.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BlogsProject.Infrastructure.Sql;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Blog> Blogs => Set<Blog>();
    public DbSet<Post> Posts => Set<Post>();
    public DbSet<Comment> Comments => Set<Comment>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // ---------------- BLOG ----------------
        modelBuilder.Entity<Blog>()
            .HasKey(b => b.Id);

        modelBuilder.Entity<Blog>()
            .Property(b => b.Id)
            .ValueGeneratedNever();

        // ---------------- POST ----------------
        modelBuilder.Entity<Post>()
            .HasKey(p => p.Id);

        modelBuilder.Entity<Post>()
            .Property(p => p.Id)
            .ValueGeneratedNever();

        modelBuilder.Entity<Post>()
            .HasOne<Blog>()
            .WithMany()
            .HasForeignKey(p => p.BlogId)
            .OnDelete(DeleteBehavior.Cascade);

        // ---------------- COMMENT ----------------
        modelBuilder.Entity<Comment>()
            .HasKey(c => c.Id);

        modelBuilder.Entity<Comment>()
            .Property(c => c.Id)
            .ValueGeneratedNever();

        modelBuilder.Entity<Comment>()
            .HasOne<Post>()
            .WithMany(p => p.Comments)
            .HasForeignKey(c => c.PostId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}