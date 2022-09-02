namespace DataAccessLibrary.DataAccess
{
    public interface IUserRepository
    {
        void SetContext(UsersContext db);
        Task CreateNewUserAsync(Models.User user);
        Task<Models.User> GetUserAsync(string username);
        Task UpdateUserAsync(Models.User updatedUser);
        Task SaveListAsync();
    }
}