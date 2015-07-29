using NailShop.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace NailShop.Business
{
    public class ProductHotBO : RepositoryData<vw_ProductHot>, IProductHot
    {
        public bool Save(ProductHot product)
        {
            using (var db = new NailShopEntities())
            {
                if (product.ProductID == -1)
                {
                    db.ProductHots.Add(product);
                    db.SaveChanges();
                    return true;
                }
                else
                {
                    db.Entry(product).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    return true;
                }
            }
        }

        public bool Delete(long ID)
        {
            using (var db = new NailShopEntities())
            {
                var row = db.ProductHots.Find(ID);
                db.Entry(row).State = System.Data.Entity.EntityState.Deleted;
                db.SaveChanges();
                return true;
            }
        }

        public vw_ProductHot GetData(long ID)
        {
            using (var db = new NailShopEntities())
            {
                var select = from c in db.vw_ProductHot
                             where c.ProductID == ID
                             select c;
                return select.First();
            }
        }

        public List<vw_ProductHot> GetData(int StoreID)
        {
            using (var db = new NailShopEntities())
            {
                if (StoreID != -1)
                {
                    var select = from c in db.vw_ProductHot
                                 where c.StoreID == StoreID
                                 orderby c.StoreID, c.ProductName
                                 select c;
                    return select.ToList();
                }
                else
                {
                    var select = from c in db.vw_ProductHot
                                 orderby c.StoreID, c.ProductName
                                 select c;
                    return select.ToList();
                }
            }
        }
    }
}
