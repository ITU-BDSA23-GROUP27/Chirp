using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Chirp.Infrastructure.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace Chirp.Infrastructure;

public sealed class ChirpDbContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
{
    public DbSet<Cheep> Cheeps => Set<Cheep>();
    public DbSet<Follower> Followers => Set<Follower>();
    public ChirpDbContext(DbContextOptions<ChirpDbContext> options): base(options)
    {
    }
    
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    modelBuilder.Entity<Cheep>().Property(c => c.Text).HasMaxLength(160);
    modelBuilder.Entity<User>().Property(u => u.Name).HasMaxLength(50);
    modelBuilder.Entity<User>().Property(u => u.Email).HasMaxLength(50);
    modelBuilder.Entity<Follower>().HasKey(f => new {f.FollowerId, f.FolloweeId});
    
    modelBuilder.Entity<IdentityUserLogin<Guid>>().HasKey(u => u.UserId);
    modelBuilder.Entity<IdentityUserRole<Guid>>().HasKey(u => u.UserId);
    modelBuilder.Entity<IdentityUserToken<Guid>>().HasKey(u => u.UserId);
}
}



