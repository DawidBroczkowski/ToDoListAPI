using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLibrary.DataAccess
{
    public class ListContext : DbContext
    {
        public ListContext(DbContextOptions options) : base(options) { }
        public DbSet<Models.User> Users { get; set; }
        public DbSet<Models.Task> Tasks { get; set; }
        public DbSet<Models.TodoList> TodoLists { get; set; }
        public DbSet<Models.Invite> Invites { get; set; }
        public DbSet<Models.Collab> Collabs { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder
                .Entity<Models.TodoList>()
                .HasMany(x => x.TaskList)
                .WithOne(x => x.TodoList)
                .OnDelete(DeleteBehavior.Cascade);
            builder
                .Entity<Models.Invite>()
                .HasOne(x => x.TargetUser)
                .WithMany(x => x.Invites)
                .OnDelete(DeleteBehavior.Cascade);
            builder
                .Entity<Models.User>()
                .HasMany(x => x.CollabLists)
                .WithOne(x => x.User)
                .OnDelete(DeleteBehavior.NoAction);
            builder
                .Entity<Models.TodoList>()
                .HasMany(x => x.Collaborations)
                .WithOne(x => x.TodoList)
                .OnDelete(DeleteBehavior.NoAction);
            builder
                .Entity<Models.Collab>()
                .HasOne(x => x.User)
                .WithMany(x => x.CollabLists)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
