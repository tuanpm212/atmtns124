using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NailShop.DataAccess;

namespace NailShop.Business
{
    public class CustomerBO: RepositoryData<Customer>, ICustomer
    {
        public Customer Login(string LoginName, string Password)
        {
            using (var db = new NailShopEntities())
            {
                var select = from c in db.Customers
                             where c.LoginName == LoginName && c.Password == Password
                             select c;

                if (select.Count() > 0)
                    return select.First();
                return null;
            }
        }

        public Store StoreLogin(string LoginName, string Password)
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

        public bool ForgetPassword(string Email, string NewPassword)
        {
            using (var db = new NailShopEntities())
            {
                var select = from c in db.Stores
                             where c.Email == Email
                             select c;
                if (select.Count() > 0)
                {
                    select.First().Password = NewPassword;
                    db.Entry(select.First()).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    return true;
                }

                var selectCustomer = from c in db.Customers
                             where c.Email == Email
                             select c;
                if (selectCustomer.Count() > 0)
                {
                    select.First().Password = NewPassword;
                    db.Entry(select.First()).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    return true;
                }
                return false;
            }
        }
    }
}
