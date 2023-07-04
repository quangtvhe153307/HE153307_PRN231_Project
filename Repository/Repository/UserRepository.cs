using BusinessObjects;
using DataAccess;
using Repository.IRepository;

namespace Repository.Repository
{
    public class UserRepository : IUserRepository
    {
        public void SaveUser(User user) => UserDAO.SaveUser(user);
        public void UpdateUser(User user) => UserDAO.UpdateUser(user);
        public List<User> GetUsers() => UserDAO.GetUsers();
        public User GetUserById(int id) => UserDAO.FindUserById(id);
        public void DeleteUser(User user) => UserDAO.DeleteUser(user);
        public User Authenticate(string email, string password) => UserDAO.Authenticate(email, password);
        public bool ContainRefreshToken(string token) => RefreshtokenDAO.ContainRefreshToken(token);
        public User GetUserByRefreshToken(string token) => UserDAO.GetUserByRefreshToken(token);
        public User GetUserByEmail(string email) => UserDAO.FindUserWithEmail(email);
    }
}