namespace DataAccessLibrary.DataAccess.DbData
{
    public interface IRepository
    {
        /// <summary>
        /// Gets a single todo list and it's entire task list.
        /// </summary>
        /// <param name="username">Calling user's username, used for authentication.</param>
        /// <param name="listId">ID of the todo list.</param>
        /// <returns>A todo list with the task list included.</returns>
        Task<Models.TodoList?>? GetTodoListAndTaskListAsync(string username, Guid? listId);
        /// <summary>
        /// Gets a single todo list.
        /// </summary>
        /// <param name="username">Calling user's username, used for authentication.</param>
        /// <param name="listId">ID of the todo list.</param>
        /// <returns>A todo list.</returns>
        Task<Models.TodoList?>? GetTodoListAsync(string username, Guid? listId);
        /// <summary>
        /// Gets a list of users permitted to use the todo list.
        /// </summary>
        /// <param name="listId">ID of the todo list.</param>
        /// <returns>A list of users permitted to use the todo list.</returns>
        Task<List<Models.User?>?>? GetTodoListCollaboratorsAsync(Guid? listId);
        /// <summary>
        /// Gets the owner of a list.
        /// </summary>
        /// <param name="listId">ID of the todo list.</param>
        /// <returns>The user who owns the list.</returns>
        Task<Models.User?>? GetTodoListOwnerAsync(Guid? listId);
        /// <summary>
        /// Gets an user.
        /// </summary>
        /// <param name="username">User's username</param>
        /// <returns>An user.</returns>
        Task<Models.User?>? GetUserAsync(string username);
        /// <summary>
        /// Gets a list of todo lists, that the user is a collaborator in.
        /// </summary>
        /// <param name="username">User's username</param>
        /// <returns>A list of user's collab lists.</returns>
        Task<List<Models.TodoList?>?>? GetUserCollabTodoListsAsync(string username);
        /// <summary>
        /// Gets a list of todo lists, that the user owns.
        /// </summary>
        /// <param name="username">User's username</param>
        /// <returns>A list of user's own todo lists.</returns>
        Task<List<Models.TodoList?>?>? GetUserOwnTodoListsAsync(string username);
        /// <summary>
        /// Gets a list of invites addressed to the user.
        /// </summary>
        /// <param name="username">User's username</param>
        /// <returns>A list of invites.</returns>
        Task<List<Models.Invite?>?>? GetUserInvitesAsync(string username);
        /// <summary>
        /// Saves changes made in the repository.
        /// </summary>
        /// <returns></returns>
        Task SaveListAsync();
        /// <summary>
        /// Injects the ListContext.
        /// </summary>
        /// <param name="db">ListContext injected in the controller.</param>
        void SetContext(ListContext db);
        /// <summary>
        /// Adds a new user to the repository.
        /// </summary>
        /// <param name="username">User's username.</param>
        /// <param name="passwordHash">Generated password hash.</param>
        /// <param name="passwordSalt">Generated password salt.</param>
        void CreateNewUser(string username, byte[] passwordHash, byte[] passwordSalt);
        /// <summary>
        /// Adds a new todo list to the repository.
        /// </summary>
        /// <param name="username">Owner's username.</param>
        /// <param name="listName">List's name.</param>
        /// <param name="description">List's description</param>
        /// <returns></returns>
        Task CreateNewTodoListAsync(string username, string listName, string? description);
        /// <summary>
        /// Removes a todo list from the repository.
        /// </summary>
        /// <param name="todoList">The todo list that is being removed.</param>
        void RemoveTodoList(Models.TodoList todoList);
        /// <summary>
        /// Invites a user to be a collaborator in a todo list.
        /// </summary>
        /// <param name="invitingUsername">Username of the user inviting.</param>
        /// <param name="receivingUsername">Username of the user being invited.</param>
        /// <param name="listId">ID of the todo list.</param>
        /// <returns><see langword="true"/> if the invite succeeded, <see langword="false"/> if the user was not found.</returns>
        Task<bool> TryInviteToCollabAsync(string invitingUsername, string receivingUsername, Guid? listId);
        /// <summary>
        /// Removes a user from collaborators of a todo list.
        /// </summary>
        /// <param name="removedUsername">Username of the user being removed.</param>
        /// <param name="listId">ID of the todo list.</param>
        /// <returns><see langword="true"/> if the removal succeeded, <see langword="false"/> if the user or collaboration was not found.</returns>
        Task<bool> TryRemoveFromCollabAsync(string removedUsername, Guid? listId);
        /// <summary>
        /// Accepts an invite, removes it from the repository and adds the user to the todo list's collaborators.
        /// </summary>
        /// <param name="username">Username of the user accepting the invite.</param>
        /// <param name="inviteId">ID of the invite.</param>
        /// <returns><see langword="true"/> if the acceptation succeeded, <see langword="false"/> if the invite was not found.</returns>
        Task<bool> TryAcceptInvite(string username, Guid? inviteId);
        /// <summary>
        /// Declines an invite and removes it from the repository.
        /// </summary>
        /// <param name="username">Username of the user declining the invite.</param>
        /// <param name="inviteId">ID of the invite.</param>
        /// <returns><see langword="true"/> if the declination succeeded, <see langword="false"/> if the invite was not found.</returns>
        Task<bool> TryDeclineInvite(string username, Guid? inviteId);
        /// <summary>
        /// Gets a todo list with a single task in the task list.
        /// </summary>
        /// <param name="username">User's username.</param>
        /// <param name="listId">ID of the todo list.</param>
        /// <param name="taskId">ID of the task.</param>
        /// <returns>A todo list with a single task.</returns>
        Task<Models.TodoList?>? GetTodoListAndSingleTaskAsync(string username, Guid? listId, Guid? taskId);
    }
}