using BusinessObjects;
using Microsoft.EntityFrameworkCore;

namespace DataAccess
{
    public class UserDAO
    {
        public static List<User> GetUsers()
        {
            var listUsers = new List<User>();
            try
            {
                using (var context = new MyDbContext())
                {
                    listUsers = context.Users
                        .ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return listUsers;
        }
        public static User FindUserById(int prodId)
        {
            User user = null;
            try
            {
                using (var context = new MyDbContext())
                {
                    user = context.Users
                        .SingleOrDefault(x => x.UserId == prodId);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return user;
        }
        public static void SaveUser(User user)
        {
            try
            {
                using (var context = new MyDbContext())
                {
                    context.Users.Add(user);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public static void UpdateUser(User user)
        {
            try
            {
                using (var context = new MyDbContext())
                {
                    //context.Entry<User>(user).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    context.Users.Update(user);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public static void DeleteUser(User user)
        {
            try
            {
                using (var context = new MyDbContext())
                {
                    var p1 = context.Users.SingleOrDefault(x => x.UserId == user.UserId);
                    context.Users.Remove(p1);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public static User Authenticate(string email, string password)
        {
            User user = null;
            try
            {
                using(var context = new MyDbContext())
                {
                    user = context.Users
                        .Include(x => x.RefreshTokens)
                        .SingleOrDefault(x => x.Email.Equals(email) && x.Password.Equals(password));
                }
            } catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return user;
        }
        public static User GetUserByRefreshToken(string token) {
            User user = null;
            try
            {
                using (var context = new MyDbContext())
                {
                    user = context.Users
                        .Include(x => x.RefreshTokens)
                        .SingleOrDefault(x => x.RefreshTokens.Select(r => r.Token).Contains(token));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return user;
        }

    }
}