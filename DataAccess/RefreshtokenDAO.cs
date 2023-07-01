using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class RefreshtokenDAO
    {
        public static bool ContainRefreshToken(string token)
        {
            bool exist = false;
            try
            {
                using (var context = new MyDbContext())
                {
                    exist = context.RefreshTokens
                        .Any(x => x.Token.Equals(token));
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return exist;
        }
        public static void SaveRefreshtoken(RefreshToken refreshtoken)
        {
            try
            {
                using (var context = new MyDbContext())
                {
                    context.RefreshTokens.Add(refreshtoken);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
