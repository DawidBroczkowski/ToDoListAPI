using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DataAccessLibrary.DataAccess.JsonData
{
    /// <summary>
    /// Doesn't work for now
    /// </summary>

    public class JsonUserRepository// : IUserRepository
    {
        //private List<Models.User> _users;

        //public JsonUserRepository()
        //{
        //    _users = new List<Models.User>();
        //    LoadList();
        //}

        ///// <summary>
        ///// Gets an user from the list by username.
        ///// </summary>
        ///// <param name="username">Unique username.</param>
        ///// <returns>User with given username.</returns>
        //public async Task<Models.User> GetUserAsync(string username)
        //{
        //    var user = _users.FirstOrDefault(x => x.Username == username);
        //    return user;
        //}

        ///// <summary>
        ///// Adds a new user to the list of users.
        ///// </summary>
        ///// <param name="user">User being added to the list.</param>
        ///// <returns></returns>
        //public async Task<bool> TryCreateNewUserAsync(Models.User user)
        //{
        //    if (_users.FirstOrDefault(x => x.Username == user.Username) is not null)
        //    {
        //        return false;
        //    }
        //    _users.Add(user);
        //    await SaveListAsync();
        //    return true;
        //}

        ///// <summary>
        ///// Saves changes to the json file.
        ///// </summary>
        //public async Task SaveListAsync()
        //{
        //    foreach (var user in _users)
        //    {
        //        if (user.TodoLists.Last().Id == Guid.Empty)
        //        {
        //            user.TodoLists.Last().Id = Guid.NewGuid();
        //        }
        //    }

        //    using FileStream stream = File.Create("Users.json");
        //    await JsonSerializer.SerializeAsync(stream, _users);
        //    await stream.DisposeAsync();
        //}

        ///// <summary>
        ///// Assigns a Guid to the last task in the list, if it's empty.
        ///// </summary>
        ///// <param name="updatedUser">User that is being updated.</param>
        ///// <returns></returns>
        //public async Task UpdateUserAsync(Models.User updatedUser)
        //{
        //    if(updatedUser.TodoLists.Last().Id == Guid.Empty)
        //    {
        //        updatedUser.TodoLists.Last().Id = Guid.NewGuid();
        //    }
        //    return;
        //}

        ///// <summary>
        ///// Not implemented.
        ///// </summary>
        //public void SetContext(UsersContext db)
        //{
        //    return;
        //}

        ///// <summary>
        ///// Loads data from the Json file.
        ///// </summary>
        //private void LoadList()
        //{
        //    var fileName = "Users.json";

        //    if (!File.Exists(fileName))
        //    {
        //        File.Create(fileName);
        //    }

        //    if (new FileInfo(fileName).Length == 0)
        //    {
        //        return;
        //    }

        //    string jsonString = File.ReadAllText("Users.json");
        //    _users = JsonSerializer.Deserialize<List<Models.User>>(jsonString);

        //    foreach (var user in _users)
        //    {
        //        if (user.TodoLists is null)
        //        {
        //            user.TodoLists = new();
        //        }
        //    }
        //}
    }
}
