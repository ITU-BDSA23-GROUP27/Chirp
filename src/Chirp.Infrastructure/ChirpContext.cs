using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Chirp.Infrastructure.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace Chirp.Infrastructure;

public sealed class ChirpContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
{
    public DbSet<Cheep> Cheeps => Set<Cheep>();
    public DbSet<Follower> Followers => Set<Follower>();
    public DbSet<Reaction> Reactions => Set<Reaction>();
    public ChirpContext(DbContextOptions<ChirpContext> options): base(options)
    {
    }
    
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    modelBuilder.Entity<Cheep>().Property(c => c.Text).HasMaxLength(160);
    
    modelBuilder.Entity<User>().Property(u => u.Name).HasMaxLength(50);
    modelBuilder.Entity<User>().Property(u => u.Email).HasMaxLength(50);
    
    modelBuilder.Entity<Follower>().HasKey(f => new {f.FollowerId, f.FolloweeId});
    
    /*modelBuilder.Entity<Follower>()
        .HasOne(f => f.FollowerUser)
        .WithMany()
        .HasForeignKey(f => f.FollowerId)
        .OnDelete(DeleteBehavior.Restrict);  

    modelBuilder.Entity<Follower>()
        .HasOne(f => f.FolloweeUser)
        .WithMany()
        .HasForeignKey(f => f.FolloweeId)
        .OnDelete(DeleteBehavior.Restrict);     
    
    modelBuilder.Entity<Reaction>()
        .HasOne(r => r.Cheep)
        .WithMany()
        .HasForeignKey(r => r.CheepId)
        .OnDelete(DeleteBehavior.Cascade); */

    
    modelBuilder.Entity<Reaction>().HasKey(r => new {r.UserId, r.CheepId, r.ReactionType, r.ReactionContent});
    modelBuilder.Entity<Reaction>().Property(r => r.ReactionContent).HasMaxLength(160);
    
    modelBuilder.Entity<IdentityUserLogin<Guid>>().HasKey(u => u.UserId);
    modelBuilder.Entity<IdentityUserRole<Guid>>().HasKey(u => u.UserId);
    modelBuilder.Entity<IdentityUserToken<Guid>>().HasKey(u => u.UserId);
}
}



