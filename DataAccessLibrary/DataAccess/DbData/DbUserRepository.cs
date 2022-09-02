using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DataAccessLibrary.DataAccess.DbData
{
    public class DbUserRepository : IUserRepository
    {
        private UsersContext _db;

        /// <summary>
        /// Injects the UsersContext.
        /// </summary>
        /// <param name="db">UsersContext injected in the controller.</param>
        public void SetContext(UsersContext db)
        {
            _db = db;
        }

        /// <summary>
        /// Adds a new user to the database.
        /// </summary>
        /// <param name="user">User being added to the database.</param>
        /// <returns></returns>
        public async Task CreateNewUserAsync(Models.User user)
        {
            await _db.Users.AddAsync(user);
        }

        /// <summary>
        /// Gets an user from the database by username.
        /// </summary>
        /// <param name="username">Unique username.</param>
        /// <returns>User with given username.</returns>
        public async Task<Models.User> GetUserAsync(string username)
        {
            var user = await _db.Users.Include(t => t.TodoList).FirstOrDefaultAsync(u => u.Username == username);
            return user;
        }

        /// <summary>
        /// Saves changes to the database.
        /// </summary>
        /// <param name="updatedUser">User that is being updated.</param>
        /// <returns></returns>
        public async Task UpdateUserAsync(Models.User updatedUser)
        {
            _db.Users.Update(updatedUser);
            await _db.SaveChangesAsync();
        }

        /// <summary>
        /// Not implemented.
        /// </summary>
        /// <returns></returns>
        public async Task SaveListAsync()
        {
            return;
        }

        /// <summary>
        /// Loads users from json file to a list. Used to migrate from a Json repository to a Database repository.
        /// </summary>
        /// <returns></returns>
        private List<Models.User> LoadFromJson()
        {
            var fileName = "Users.json";

            if (!File.Exists(fileName))
            {
                File.Create(fileName);
            }

            if (new FileInfo(fileName).Length == 0)
            {
                return null;
            }

            string jsonString = File.ReadAllText("Users.json");
            var users = JsonSerializer.Deserialize<List<Models.User>>(jsonString);

            foreach (var user in users)
            {
                if (user.TodoList is null)
                {
                    user.TodoList = new();
                }
            }

            return users;
        }
    }
}
