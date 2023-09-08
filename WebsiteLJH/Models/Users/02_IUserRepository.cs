//[User][3]
namespace WebsiteLJH
{
    public interface IUserRepository
    {
        void AddUser(string userId, string password);
        UserViewModel GetUserByUserId(string userId);
        bool IsCorrectUser(string userId, string password);
        void ModifyUser(int uid, string userId, string password);
    }
}
