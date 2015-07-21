using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NailShop.DataAccess;

namespace NailShop.Business
{
    public interface ICustomer: IRepository<Customer>
    {
        Customer Login(string LoginName, string Password);
        bool ForgetPassword(string Email, string NewPassword);
    }
}
