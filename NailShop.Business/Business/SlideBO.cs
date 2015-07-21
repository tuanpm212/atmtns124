using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NailShop.DataAccess;
using System.Transactions;

namespace NailShop.Business
{
    public class SlideBO : RepositoryData<vw_Slide>, ISlide
    {
        public bool AddNew(Slide slide, SlideLang slideLang)
        {
            using (var db = new NailShopEntities())
            {
                using (TransactionScope transcope = new TransactionScope())
                {
                    try
                    {
                        db.Slides.Add(slide);
                        db.SaveChanges();
                        slideLang.SlideID = slide.SlideID;
                        db.SlideLangs.Add(slideLang);
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

        public bool Edit(Slide slide, SlideLang slideLang)
        {
            using (var db = new NailShopEntities())
            {
                using (TransactionScope transcope = new TransactionScope())
                {
                    try
                    {
                        db.Entry(slide).State = System.Data.Entity.EntityState.Modified;
                        db.Entry(slideLang).State = System.Data.Entity.EntityState.Modified;
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
