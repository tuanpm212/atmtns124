using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NailShop.Business;
using NailShop.DataAccess;
using System.Transactions;

namespace NailShop.Business
{
    public class BrandBO: RepositoryData<vw_Brand>, IBrand
    {
        public bool Save(Brand brand, BrandLang brandLang)
        {
            using (var db = new NailShopEntities())
            {
                using (TransactionScope transcope = new TransactionScope())
                {
                    try
                    {
                        if (brand.BrandID == -1)
                        {
                            db.Brands.Add(brand);
                            db.SaveChanges();
                            brandLang.BrandID = brand.BrandID;
                            db.BrandLangs.Add(brandLang);
                            db.SaveChanges();
                            transcope.Complete();
                            return true;
                        }
                        else
                        {
                            db.Entry(brand).State = System.Data.Entity.EntityState.Modified;
                            db.Entry(brandLang).State = System.Data.Entity.EntityState.Modified;
                            db.SaveChanges();
                            transcope.Complete();
                            return true;
                        }
                    }
                    catch
                    {
                        transcope.Dispose();
                        return false;
                    }
                }
            }
        }

        public bool Delete(long ID)
        {
            using (var db = new NailShopEntities())
            {
                var row = db.Brands.Find(ID);
                db.Entry(row).State = System.Data.Entity.EntityState.Deleted;
                db.SaveChanges();
                return true;
            }
        }

        public vw_Brand GetData(string LangID, long ID)
        {
            using (var db = new NailShopEntities())
            {
                var select = from c in db.vw_Brand
                             where c.LangID == LangID && c.BrandID == ID
                             select c;
                return select.First();
            }
        }

        public List<vw_Brand> GetData(string LangID)
        {
            using (var db = new NailShopEntities())
            {
                var select = from c in db.vw_Brand
                             where c.LangID == LangID
                             select c;
                return select.ToList();
            }
        }

    }
}
