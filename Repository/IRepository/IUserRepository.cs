using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.IRepository
{
    public interface IUserRepository
    {
        void SaveUser(User user);
        User GetUserById(int id);
        void DeleteUser(User user);
        void UpdateUser(User user);
        List<User> GetUsers();
        User Authenticate(string email, string password);
        User GetUserByRefreshToken(string token);
        bool ContainRefreshToken(string token);
    }
}
