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
        public bool AddNew(Brand brand, BrandLang brandLang)
        {
            using (var db = new NailShopEntities())
            {
                using (TransactionScope transcope = new TransactionScope())
                {
                    try
                    {
                        db.Brands.Add(brand);
                        db.SaveChanges();
                        brandLang.BrandID = brand.BrandID;
                        db.BrandLangs.Add(brandLang);
                        db.SaveChanges();
                        transcope.Complete();
                        return true;
                    }
                    catch
                    {
                        transcope.Dispose();
                        return false;
                    }
                }
            }
        }

        public bool Edit(Brand brand, BrandLang brandLang)
        {
            using (var db = new NailShopEntities())
            {
                using (TransactionScope transcope = new TransactionScope())
                {
                    try
                    {
                        db.Entry(brand).State = System.Data.Entity.EntityState.Modified;
                        db.Entry(brandLang).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();
                        transcope.Complete();
                        return true;
                    }
                    catch
                    {
                        transcope.Dispose();
                        return false;
                    }
                }
            }
        }
    }
}
