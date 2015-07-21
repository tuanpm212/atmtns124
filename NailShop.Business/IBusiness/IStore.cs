using NailShop.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NailShop.Business
{
    public interface IStore : IRepository<Store>
    {
        Store Login(string LoginName, string Password);
    }
}
