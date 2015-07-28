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
        public vw_Slide GetData(long ID)
        {
            using (var db = new NailShopEntities())
            {
                var select = from c in db.vw_Slide
                             where c.SlideID == ID
                             select c;
                return select.First();
            }
        }

        public bool Delete(long ID)
        {
            using (var db = new NailShopEntities())
            {
                var row = db.Slides.Find(ID);
                db.Entry(row).State = System.Data.Entity.EntityState.Deleted;
                db.SaveChanges();
                return true;
            }
        }

        public List<vw_Slide> GetSlide(string LangID, string Type)
        {
            using (var db = new NailShopEntities())
            {
                var select = from c in db.vw_Slide
                             where c.LangID == LangID && c.Type == Type
                             select c;
                return select.ToList();
            }
        }

        public bool SaveSlide(Slide slide, SlideLang slideLang)
        {
            using (var db = new NailShopEntities())
            {
                using (TransactionScope transcope = new TransactionScope())
                {
                    try
                    {
                        if (slide.SlideID == -1)
                        {
                            db.Slides.Add(slide);
                            db.SaveChanges();
                            slideLang.SlideID = slide.SlideID;
                            db.SlideLangs.Add(slideLang);
                            db.SaveChanges();
                            transcope.Complete();
                            return true;
                        }
                        else
                        {
                            db.Entry(slide).State = System.Data.Entity.EntityState.Modified;
                            db.Entry(slideLang).State = System.Data.Entity.EntityState.Modified;
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
    }
}
