using NailShop.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NailShop.Business
{
    public interface IProductHot : IRepository<vw_ProductHot>
    {
        bool Save(ProductHot product);

        bool Delete(long ID);

        vw_ProductHot GetData(long ID);

        List<vw_ProductHot> GetData(int StoreID);
    }
}
