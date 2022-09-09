using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DataAccessLibrary.DataAccess.DbData
{
    public class DbRepository : IRepository
    {
        private ListContext _db;

        public void SetContext(ListContext db)
        {
            _db = db;
        }

        public void CreateNewUser(string username, byte[] passwordHash, byte[] passwordSalt)
        {
            Models.User user = new()
            {
                Username = username,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt
            };
            _db.Users.Add(user);
        }

        public async Task<Models.User> GetUserAsync(string username)
        {
            var user = await _db.Users.IgnoreAutoIncludes().FirstOrDefaultAsync(u => u.Username == username);
            return user;
        }

        public async Task<Models.TodoList> GetTodoListAsync(string username, Guid? listId)
        {
            var todoList = await _db.TodoLists
                .IgnoreAutoIncludes()
                .Where(x => x.Collaborations.Any(x => x.User.Username == username) || x.Owner.Username == username)
                .FirstOrDefaultAsync(x => x.Id == listId);
            return todoList;
        }

        public async Task<Models.TodoList> GetTodoListAndTaskListAsync(string username, Guid? listId)
        {
            var todoList = await _db.TodoLists
                .IgnoreAutoIncludes()
                .Include(x => x.TaskList)
                .Where(x => x.Collaborations.Any(x => x.User.Username == username) || x.Owner.Username == username)
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
            var todoList = await _db.TodoLists
                .IgnoreAutoIncludes()
                .Include(x => x.Collaborations)
                .ThenInclude(x => x.User)
                .FirstOrDefaultAsync(x => x.Id == listId);
            var users = todoList.Collaborations.Select(x => x.User).ToList();
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
            var todoLists = await _db.TodoLists.IgnoreAutoIncludes().Where(x => x.Collaborations.Any(x => x.User.Username == username)).ToListAsync();
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
                .Where(x => x.Collaborations.Any(x => x.User.Username == username) || x.Owner.Username == username)   
                .Include(x => x.TaskList.Where(x => x.Id == taskId))
                .FirstOrDefaultAsync();
            return todoLists;
        }

        public async Task CreateNewTodoListAsync(string username, string name, string? description)
        {
            Models.TodoList todoList = new()
            {
                Owner = await GetUserAsync(username),
                Collaborations = new(),
                Name = name,
                Description = description
            };

            await _db.TodoLists.AddAsync(todoList);
        }

        public void RemoveTodoList(Models.TodoList todoList)
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

        public async Task<bool> TryRemoveFromCollabAsync(string removedUsername, Guid? listId)
        {
            var collab = await _db.Collabs.IgnoreAutoIncludes().FirstOrDefaultAsync(x => x.TodoList.Id == listId && x.User.Username == removedUsername);
            if (collab is null)
            {
                return false;
            }
            _db.Remove(collab);
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
            todoList.Collaborations = new();

            Models.Collab collab = new();
            collab.TodoList = todoList;
            collab.User = user;

            todoList.Collaborations.Add(collab);
            user.Invites.RemoveAt(0);
            return true;
        }

        public async Task<bool> TryDeclineInvite(string username, Guid? inviteId)
        {
            var user = await _db.Users.Include(x => x.Invites.Where(x => x.InviteId == inviteId)).FirstOrDefaultAsync(x => x.Username == username);
            if (user.Invites.FirstOrDefault() is null)
            {
                return false;
            }
            user.Invites.RemoveAt(0);
            return true;
        }

        public async Task SaveListAsync()
        {
            await _db.SaveChangesAsync();
        }
    }
}
