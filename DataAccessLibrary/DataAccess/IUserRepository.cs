namespace DataAccessLibrary.DataAccess
{
    public interface IUserRepository
    {
        Task<Models.TodoList> GetTodoListAndTaskListAsync(string username, Guid? listId);
        Task<Models.TodoList> GetTodoListAsync(string username, Guid? listId);
        Task<List<Models.User>> GetTodoListCollaboratorsAsync(Guid? listId);
        Task<Models.User> GetTodoListOwnerAsync(Guid? listId);
        Task<Models.User> GetUserAsync(string username);
        Task<List<Models.TodoList>> GetUserCollabTodoListsAsync(string username);
        Task<List<Models.TodoList>> GetUserOwnTodoListsAsync(string username);
        Task<List<Models.Invite>> GetUserInvitesAsync(string username);
        Task SaveListAsync();
        void SetContext(UsersContext db);
        Task<bool> TryCreateNewUserAsync(Models.User user);
        Task UpdateUserAsync(Models.User updatedUser);
        Task CreateNewTodoListAsync(string username, string name, string? description);
        Task RemoveTodoListAsync(Models.TodoList todoList);
        Task<bool> TryInviteToCollabAsync(string invitingUsername, string receivingUsername, Guid? listId);
        Task<bool> TryAcceptInvite(string username, Guid? inviteId);
        Task<Models.TodoList> GetTodoListAndSingleTaskAsync(string username, Guid? listId, Guid? taskId);
        void DeleteObject(object obj);
    }
}