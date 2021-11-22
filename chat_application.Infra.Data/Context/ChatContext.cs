using Microsoft.EntityFrameworkCore;
using chat_application.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chat_application.Infra.Data.Context
{
    public class ChatContext : DbContext
    {
        public ChatContext(DbContextOptions<ChatContext> options)
          : base(options)
        { }

        public DbSet<User> Users { get; set; }
        public DbSet<ChatMessage> Messages { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<User>().HasKey(m => m.key);
            builder.Entity<ChatMessage>().HasKey(m => m.key);
            builder.Entity<ChatMessage>().HasOne(m => m.from).WithMany().HasForeignKey(u => u.fromId);
            builder.Entity<ChatMessage>().HasOne(m => m.to).WithMany().HasForeignKey(u => u.toId);

            base.OnModelCreating(builder);
        }
    }
}
