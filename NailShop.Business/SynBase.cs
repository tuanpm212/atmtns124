using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NailShop.DataAccess;

namespace NailShop.Business
{
    public class SynBase
    {
        public string CheckLoginTest(string LoginName, string Password)
        {
            try
            {
                using (var context = new NailShopEntities())
                {
                    var select = from c in context.Stores
                                 where c.LoginName == LoginName && c.Password == Password
                                 select c;

                    if (select.Count() == 1)
                        return "Ok";
                    return "false";
                }
            }
            catch(Exception ex)
            {
                return ex.Message;
            }
        }

        public bool CheckLogin(string LoginName, string Password)
        {
            try
            {
                using (var context = new NailShopEntities())
                {
                    var select = from c in context.Stores
                                 where c.LoginName == LoginName && c.Password == Password
                                 select c;

                    if (select.Count() == 1)
                        return true;
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
