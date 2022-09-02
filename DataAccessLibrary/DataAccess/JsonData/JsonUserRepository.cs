using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DataAccessLibrary.DataAccess.JsonData
{
    public class JsonUserRepository : IUserRepository
    {
        private List<Models.User> _users;

        public JsonUserRepository()
        {
            _users = new List<Models.User>();
            LoadList();
        }

        /// <summary>
        /// Gets an user from the list by username.
        /// </summary>
        /// <param name="username">Unique username.</param>
        /// <returns>User with given username.</returns>
        public async Task<Models.User> GetUserAsync(string username)
        {
            var user = _users.Find(x => x.Username == username);
            return user;
        }

        /// <summary>
        /// Adds a new user to the list of users.
        /// </summary>
        /// <param name="user">User being added to the list.</param>
        /// <returns></returns>
        public async Task CreateNewUserAsync(Models.User user)
        {
            _users.Add(user);
            await SaveListAsync();
        }

        /// <summary>
        /// Saves changes to the json file.
        /// </summary>
        public async Task SaveListAsync()
        {
            using FileStream stream = File.Create("Users.json");
            await JsonSerializer.SerializeAsync(stream, _users);
            await stream.DisposeAsync();
        }

        /// <summary>
        /// Assigns a Guid to the last task in the list, if it's empty.
        /// </summary>
        /// <param name="updatedUser">User that is being updated.</param>
        /// <returns></returns>
        public async Task UpdateUserAsync(Models.User updatedUser)
        {
            if(updatedUser.TodoList.Last().Id == Guid.Empty)
            {
                updatedUser.TodoList.Last().Id = Guid.NewGuid();
            }
            return;
        }

        /// <summary>
        /// Not implemented.
        /// </summary>
        public void SetContext(UsersContext db)
        {
            return;
        }

        /// <summary>
        /// Loads data from the Json file.
        /// </summary>
        private void LoadList()
        {
            var fileName = "Users.json";

            if (!File.Exists(fileName))
            {
                File.Create(fileName);
            }

            if (new FileInfo(fileName).Length == 0)
            {
                return;
            }

            string jsonString = File.ReadAllText("Users.json");
            _users = JsonSerializer.Deserialize<List<Models.User>>(jsonString);

            foreach (var user in _users)
            {
                if (user.TodoList is null)
                {
                    user.TodoList = new();
                }
            }
        }
    }
}
