using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DataAccessLibrary
{
    public class JsonUserRepository : IUserRepository
    {
        private List<Models.User> _users = new();

        public JsonUserRepository()
        {
            LoadList();
        }

        public Models.User GetUser(string username)
        {
            var user = _users.Find(x => x.Username == username);
            return user;
        }

        public void CreateNewUser(Models.User user)
        {
            _users.Add(user);
        }

        private void LoadList()
        {
            var fileName = "Users.json";

            if(!File.Exists(fileName))
            {
                File.Create(fileName);
            }

            if(new FileInfo(fileName).Length == 0)
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

        public async Task SaveListAsync()
        {
            using FileStream fileStream = File.Create("Users.json");
            await JsonSerializer.SerializeAsync(fileStream, _users);
            await fileStream.DisposeAsync();
        }
    }
}
