using NailShop.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace NailShop.Business
{
    public class WelcomeBO : RepositoryData<vw_Slide>, IWelcome
    {
        public vw_Slide GetWelcome(string LangID)
        {
            using (var db = new NailShopEntities())
            {
                var select = from c in db.vw_Slide
                             where c.LangID == LangID && c.Type == "WELCOME"
                             select c;
                return select.First();
            }
        }

        public bool SaveWelcome(Slide slide, SlideLang slideLang)
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
