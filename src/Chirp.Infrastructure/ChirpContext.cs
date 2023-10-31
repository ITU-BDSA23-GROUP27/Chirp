using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Chirp.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace Chirp.Infrastructure;

public sealed class ChirpContext : DbContext
{
    public DbSet<Cheep> Cheeps => Set<Cheep>();
    public DbSet<Author> Authors => Set<Author>();

    public ChirpContext(DbContextOptions<ChirpContext> options): base(options)
    {
    }
    
/*private static string DbPath
{
    get
    {
        string path = Path.GetTempPath();
        return Path.Join(path, "chirp.db");
    }
}*/


protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    modelBuilder.Entity<Cheep>().Property(c => c.Text).HasMaxLength(160);
    modelBuilder.Entity<Author>().Property(a => a.Name).HasMaxLength(50);
    modelBuilder.Entity<Author>().Property(c => c.Email).HasMaxLength(50);
    modelBuilder.Entity<Follower>().HasKey(f => new {f.FollowerId, f.FolloweeId});
}
}



