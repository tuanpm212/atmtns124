using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NailShop.DataAccess;
using System.Transactions;

namespace NailShop.Business
{
    public class NewsBO : RepositoryData<vw_News>, INews
    {
        public bool Save(News news, NewsLang newsLang)
        {
            using (var db = new NailShopEntities())
            {
                using (TransactionScope transcope = new TransactionScope())
                {
                    try
                    {
                        if (news.NewsID == -1)
                        {
                            db.News.Add(news);
                            db.SaveChanges();
                            newsLang.NewsID = news.NewsID;
                            db.NewsLangs.Add(newsLang);
                            db.SaveChanges();
                            transcope.Complete();
                            return true;
                        }
                        else
                        {
                            db.Entry(news).State = System.Data.Entity.EntityState.Modified;
                            db.Entry(newsLang).State = System.Data.Entity.EntityState.Modified;
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
                var row = db.News.Find(ID);
                db.Entry(row).State = System.Data.Entity.EntityState.Deleted;
                db.SaveChanges();
                return true;
            }
        }

        public vw_News GetData(string LangID, long ID)
        {
            using (var db = new NailShopEntities())
            {
                var select = from c in db.vw_News
                             where c.LangID == LangID && c.NewsID == ID
                             select c;
                return select.First();
            }
        }

        public List<vw_News> GetData(string LangID)
        {
            using (var db = new NailShopEntities())
            {
                var select = from c in db.vw_News
                             where c.LangID == LangID
                             select c;
                return select.ToList();
            }
        }
    }
}
