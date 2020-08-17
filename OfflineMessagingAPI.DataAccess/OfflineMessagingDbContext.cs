using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OfflineMessagingAPI.Entities.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace OfflineMessagingAPI.DataAccess
{
    public class OfflineMessagingDbContext : IdentityDbContext<User,IdentityRole<int>,int>
    {

        public OfflineMessagingDbContext(DbContextOptions<OfflineMessagingDbContext> options) : base(options)
        {
        }

        public DbSet<Message> Messages { get; set; }
        public DbSet<Block> Blocks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)

        {
            modelBuilder.Entity<Block>()
                    .HasOne(b => b.BlockerUser)
                    .WithMany(u => u.BlockerBlocks)
                    .HasForeignKey(b => b.BlockerUserId)
                    .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Block>()
                    .HasOne(b => b.BlockedUser)
                    .WithMany(u => u.BlockedBlocks)
                    .HasForeignKey(b => b.BlockedUserId)
                    .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Message>()
                    .HasOne(m => m.Sender)
                    .WithMany(u => u.SenderMessages)
                    .HasForeignKey(m => m.SenderId)
                    .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Message>()
                    .HasOne(m => m.Receiver)
                    .WithMany(u => u.ReceiverMessages)
                    .HasForeignKey(m => m.ReceiverId)
                    .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<User>()
                    .HasIndex(u => u.UserName)
                    .IsUnique();

            base.OnModelCreating(modelBuilder);
        }    
    }
}
