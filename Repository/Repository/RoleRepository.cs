using BusinessObjects;
using DataAccess;
using Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repository
{
    public class RoleRepository : IRoleRepository
    {
        public void SaveRole(Role role) => RoleDAO.SaveRole(role);
        public void UpdateRole(Role role) => RoleDAO.UpdateRole(role);
        public List<Role> GetRoles() => RoleDAO.GetRoles();
        public Role GetRoleById(int id) => RoleDAO.FindRoleById(id);

        public void DeleteRole(Role role) => RoleDAO.DeleteRole(role);
    }
}
