namespace DataAccessLibrary
{
    public interface IUserRepository
    {
        void CreateNewUser(Models.User user);
        Models.User GetUser(string username);
        Task SaveListAsync();
    }
}