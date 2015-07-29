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
        bool Save(Brand brand, BrandLang brandLang);

        bool Delete(long ID);

        vw_Brand GetData(string LangID, long ID);

        List<vw_Brand> GetData(string LangID);
    }
}
