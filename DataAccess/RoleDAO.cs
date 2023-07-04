using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class RoleDAO
    {
        public static List<Role> GetRoles()
        {
            var listRoles = new List<Role>();
            try
            {
                using (var context = new MyDbContext())
                {
                    listRoles = context.Roles
                        .ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return listRoles;
        }
        public static Role FindRoleById(int prodId)
        {
            Role role = null;
            try
            {
                using (var context = new MyDbContext())
                {
                    role = context.Roles
                        .SingleOrDefault(x => x.RoleId == prodId);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return role;
        }
        public static void SaveRole(Role role)
        {
            try
            {
                using (var context = new MyDbContext())
                {
                    context.Roles.Add(role);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public static void UpdateRole(Role role)
        {
            try
            {
                using (var context = new MyDbContext())
                {
                    context.Entry<Role>(role).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public static void DeleteRole(Role role)
        {
            try
            {
                using (var context = new MyDbContext())
                {
                    var p1 = context.Roles.SingleOrDefault(x => x.RoleId == role.RoleId);
                    context.Roles.Remove(p1);
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
