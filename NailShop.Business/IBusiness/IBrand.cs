using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NailShop.DataAccess;

namespace NailShop.Business
{
    public interface IBrand : IRepository<vw_Brand>
    {
        bool AddNew(Brand brand, BrandLang brandLang);
        bool Edit(Brand brand, BrandLang brandLang);
    }
}
