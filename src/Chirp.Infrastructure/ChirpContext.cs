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
    // Making every property of string to have MaxLength is generated from ChatGPT
    
    base.OnModelCreating(modelBuilder);

    foreach (var entityType in modelBuilder.Model.GetEntityTypes())
    {
        foreach (var property in entityType.GetProperties())
        {
            if (property.ClrType == typeof(string) && property.GetMaxLength() == null)
            {
                modelBuilder.Entity(entityType.ClrType)
                    .Property(property.Name)
                    .HasMaxLength(160);
            }
        }
    }
    
    modelBuilder.Entity<Cheep>().Property(c => c.Text).HasMaxLength(160);
    
    modelBuilder.Entity<User>().Property(u => u.Name).HasMaxLength(50);
    modelBuilder.Entity<User>().Property(u => u.Email).HasMaxLength(50);
    
    modelBuilder.Entity<Follower>().HasKey(f => new {f.FollowerId, f.FolloweeId});
    
    modelBuilder.Entity<Follower>()
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
        .OnDelete(DeleteBehavior.Restrict);
    
    modelBuilder.Entity<Reaction>()
        .HasOne(r => r.User)
        .WithMany()
        .HasForeignKey(r => r.UserId)
        .OnDelete(DeleteBehavior.Restrict);
    
    modelBuilder.Entity<Reaction>().HasKey(r => new {r.UserId, r.CheepId, r.ReactionType, r.ReactionContent});
    modelBuilder.Entity<Reaction>().Property(r => r.ReactionContent).HasMaxLength(160);
    
    modelBuilder.Entity<IdentityUserLogin<Guid>>().HasKey(u => u.UserId);
    modelBuilder.Entity<IdentityUserRole<Guid>>().HasKey(u => u.UserId);
    modelBuilder.Entity<IdentityUserToken<Guid>>().HasKey(u => u.UserId);
}
}



