using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Chirp.Infrastructure;

public class ChirpContext : DbContext
{
    public DbSet<Cheep> Cheeps => Set<Cheep>();
    public DbSet<Author> Authors => Set<Author>();
    
    private static string DbPath
    {
        get
        {
            string path = Path.GetTempPath();
            return Path.Join(path, "chirp.db");
        }
    }

    // The following configures EF to create a Sqlite database file in the
    // special "local" folder for your platform.
    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite($"Data Source={DbPath}");
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Cheep>().Property(c => c.Text).HasMaxLength(160);
        modelBuilder.Entity<Author>().Property(a => a.Name).HasMaxLength(50);
        modelBuilder.Entity<Author>().Property(c => c.Email).HasMaxLength(50);
        modelBuilder.Entity<Follower>().HasKey(f => new {f.FollowerId, f.FolloweeId});
    }
}

// could be in seperate classes for better overview 
public class Cheep
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid CheepId { get; set; }
    
    public required string Text { get; set; }
    public DateTime TimeStamp { get; set; }
    public required Author Author { get; set; }
}

public class Author
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid AuthorId { get; set; }
    
    public required string Name { get; set; }
    public required string Email { get; set; }
    public IEnumerable<Cheep> Cheeps { get; set; } = new List<Cheep>();
}

public class Follower
{
    public Guid FollowerId { get; set; }
    public Guid FolloweeId { get; set; }
    public required Author FollowerAuthor { get; set; }
    public required Author FolloweeAuthor { get; set; }
}