using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.IRepository
{
    public interface IRoleRepository
    {
        void SaveRole(Role role);
        Role GetRoleById(int id);
        void DeleteRole(Role role);
        void UpdateRole(Role role);
        List<Role> GetRoles();
    }
}
