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
    public class RefreshtokenRepository : IRefreshtokenRepository
    {
        public void SaveRefreshtoken(RefreshToken refreshtoken) => RefreshtokenDAO.SaveRefreshtoken(refreshtoken);
    }
}
