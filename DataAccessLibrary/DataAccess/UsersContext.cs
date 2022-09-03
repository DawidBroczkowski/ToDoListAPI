using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLibrary.DataAccess
{
    public class UsersContext : DbContext
    {
        public UsersContext(DbContextOptions options) : base(options) { }
        public DbSet<Models.User> Users { get; set; }
        public DbSet<Models.Task> Tasks { get; set; }
        public DbSet<Models.TodoList> TodoLists { get; set; }
    }
}
