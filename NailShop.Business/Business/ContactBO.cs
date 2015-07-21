using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NailShop.DataAccess;
using System.Transactions;

namespace NailShop.Business
{
    public class ContactBO: RepositoryData<Contact>, IContact
    {

    }
}
