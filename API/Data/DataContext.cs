﻿using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class DataContext(DbContextOptions options) : DbContext(options)
{
      public DbSet<AppUser> Users { get; set; }
      public DbSet<Photo> Photos { get; set; }
      public DbSet<UserLike> Likes { get; set; }

      protected override void OnModelCreating(ModelBuilder modelBuilder)
      {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserLike>()
                  .HasKey(k => new { k.SourceUserId, k.TargetUserId });

            modelBuilder.Entity<UserLike>()
                  .HasOne(s => s.SourceUser)
                  .WithMany(l => l.LikedUsers)
                  .HasForeignKey(s => s.SourceUserId)
                  .OnDelete(DeleteBehavior.Cascade); //.OnDelete(DeleteBehavior.NoAction); - for SQL Server

            modelBuilder.Entity<UserLike>()
                  .HasOne(s => s.TargetUser)
                  .WithMany(l => l.LikedByUsers)
                  .HasForeignKey(s => s.TargetUserId)
                  .OnDelete(DeleteBehavior.Cascade); //.OnDelete(DeleteBehavior.NoAction); - for SQL Server
      }
}
