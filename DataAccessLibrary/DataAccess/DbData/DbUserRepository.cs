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
        public async Task<bool> TryCreateNewUserAsync(Models.User user)
        {
            if (_db.Users.FirstOrDefault(x => x.Username == user.Username) is not null)
            {
                return false;
            }

            _db.Users.Add(user);
            _db.SaveChanges();
            return true;
        }

        /// <summary>
        /// Gets an user from the database by username.
        /// </summary>
        /// <param name="username">Unique username.</param>
        /// <returns>User with given username.</returns>
        public async Task<Models.User> GetUserAsync(string username)
        {
            var user = await _db.Users.IgnoreAutoIncludes().FirstOrDefaultAsync(u => u.Username == username);
            return user;
        }

        public async Task<Models.TodoList> GetTodoListAsync(string username, Guid? listId)
        {
            var todoList = await _db.TodoLists
                .IgnoreAutoIncludes()
                .Where(x => x.Collaborators.Any(x => x.User.Username == username) || x.Owner.Username == username)
                .FirstOrDefaultAsync(x => x.Id == listId);
            return todoList;
        }

        public async Task<Models.TodoList> GetTodoListAndTaskListAsync(string username, Guid? listId)
        {
            var todoList = await _db.TodoLists
                .IgnoreAutoIncludes()
                .Include(x => x.TaskList)
                .Where(x => x.Collaborators.Any(x => x.User.Username == username) || x.Owner.Username == username)
                .FirstOrDefaultAsync(x => x.Id == listId);
            return todoList;
        }

        public async Task<Models.User> GetTodoListOwnerAsync(Guid? listId)
        {
            var todoList = await _db.TodoLists.IgnoreAutoIncludes().Include(x => x.Owner).FirstOrDefaultAsync(x => x.Id == listId);
            return todoList.Owner;
        }

        public async Task<List<Models.User>> GetTodoListCollaboratorsAsync(Guid? listId)
        {
            var todoList = await _db.TodoLists.IgnoreAutoIncludes().Include(x => x.Collaborators).FirstOrDefaultAsync(x => x.Id == listId);
            var users = todoList.Collaborators.Select(x => x.User).ToList();
            return users;
        }

        public async Task<List<Models.TodoList>> GetUserOwnTodoListsAsync(string username)
        {
            var user = await _db.Users.IgnoreAutoIncludes().FirstOrDefaultAsync(x => x.Username == username);
            var todoLists = await _db.TodoLists.IgnoreAutoIncludes().Where(x => x.Owner.Username == username).ToListAsync();
            return todoLists;
        }

        public async Task<List<Models.TodoList>> GetUserCollabTodoListsAsync(string username)
        {
            var user = await _db.Users.IgnoreAutoIncludes().FirstOrDefaultAsync(x => x.Username == username);
            var todoLists = await _db.TodoLists.IgnoreAutoIncludes().Where(x => x.Collaborators.Any(x => x.User.Username == username)).ToListAsync();
            return todoLists;
        }

        public async Task<List<Models.Invite>> GetUserInvitesAsync(string username)
        {
            var invites = await _db.Invites.IgnoreAutoIncludes().Where(x => x.TargetUser.Username == username).ToListAsync();
            return invites;
        }

        public async Task<Models.TodoList> GetTodoListAndSingleTaskAsync(string username, Guid? listId, Guid? taskId)
        {
            var user = await _db.Users.FirstOrDefaultAsync(x => x.Username == username);
            var todoLists = await _db.TodoLists
                .IgnoreAutoIncludes()
                .Where(x => x.Id == listId)
                .Where(x => x.Collaborators.Any(x => x.User.Username == username) || x.Owner.Username == username)   
                .Include(x => x.TaskList.Where(x => x.Id == taskId))
                .FirstOrDefaultAsync();
            return todoLists;
        }

        public async Task CreateNewTodoListAsync(string username, string name, string? description)
        {
            Models.TodoList todoList = new()
            {
                Owner = await GetUserAsync(username),
                Collaborators = new(),
                Name = name,
                Description = description
            };

            await _db.TodoLists.AddAsync(todoList);
        }

        public async Task RemoveTodoListAsync(Models.TodoList todoList)
        {
            _db.TodoLists.Remove(todoList);
        }

        public async Task<bool> TryInviteToCollabAsync(string invitingUsername, string receivingUsername, Guid? listId)
        {
            var user = await GetUserAsync(receivingUsername);
            if (user is null)
            {
                return false;
            }

            Models.Invite invite = new()
            {
                InvitingUsername = invitingUsername,
                ListId = listId,
                InviteDate = DateTime.UtcNow
            };

            user.Invites = new();
            user.Invites.Add(invite);
            return true;
        }

        public async Task<bool> TryAcceptInvite(string username, Guid? inviteId)
        {
            var user = await _db.Users.Include(x => x.Invites.Where(x => x.InviteId == inviteId)).FirstOrDefaultAsync(x => x.Username == username);
            if (user.Invites.FirstOrDefault() is null)
            {
                return false;
            }
            var todoList = await _db.TodoLists.FirstOrDefaultAsync(x => x.Id == user.Invites.First().ListId);
            todoList.Collaborators = new();

            Models.Collab collab = new();
            collab.TodoList = todoList;
            collab.User = user;

            todoList.Collaborators.Add(collab);
            user.Invites.RemoveAt(0);
            return true;
        }
        
        /// <summary>
        /// Saves changes to the database.
        /// </summary>
        /// <param name="updatedUser">User that is being updated.</param>
        /// <returns></returns>
        public async Task UpdateUserAsync(Models.User updatedUser)
        {
            _db.Users.Update(updatedUser);
        }

        public void DeleteObject(object obj)
        {
            _db.Remove(obj);
        }

        public async Task SaveListAsync()
        {
            await _db.SaveChangesAsync();
        }

        /// <summary>
        /// Loads users from json file to a list. Used to migrate from a Json repository to a Database repository.
        /// </summary>
        /// <returns></returns>
        //private List<Models.User> LoadFromJson()
        //{
        //    var fileName = "Users.json";

        //    if (!File.Exists(fileName))
        //    {
        //        File.Create(fileName);
        //    }

        //    if (new FileInfo(fileName).Length == 0)
        //    {
        //        return null;
        //    }

        //    string jsonString = File.ReadAllText("Users.json");
        //    var users = JsonSerializer.Deserialize<List<Models.User>>(jsonString);

        //    foreach (var user in users)
        //    {
        //        if (user.TodoLists is null)
        //        {
        //            user.TodoLists = new();
        //        }
        //    }

        //    return users;
        //}
    }
}
