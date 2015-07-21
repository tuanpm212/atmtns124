using NailShop.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NailShop.Business
{
    public class StoreBO : RepositoryData<Store>, IStore
    {
        public Store Login(string LoginName, string Password)
        {
            using (var db = new NailShopEntities())
            {
                var select = from c in db.Stores
                             where c.LoginName == LoginName && c.Password == Password
                             select c;
                if (select.Count() > 0)
                    return select.First();
                return null;
            }
        }
    }
}
