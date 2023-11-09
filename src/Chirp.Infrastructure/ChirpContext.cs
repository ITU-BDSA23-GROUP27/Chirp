using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Chirp.Infrastructure.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace Chirp.Infrastructure;

public sealed class ChirpContext : IdentityDbContext<User>
{
    public DbSet<Cheep> Cheeps => Set<Cheep>();

    public ChirpContext(DbContextOptions<ChirpContext> options): base(options){ }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
    
    /*
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Cheep>().Property(c => c.Text).HasMaxLength(160);
        modelBuilder.Entity<User>().HasKey(u => u.UserId);
        modelBuilder.Entity<User>().Property(u => u.Name).HasMaxLength(50);
        modelBuilder.Entity<User>().Property(u => u.Email).HasMaxLength(50);
        modelBuilder.Entity<Follower>().HasKey(f => new {f.FollowerId, f.FolloweeId});
    }*/
}



