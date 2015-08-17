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
        public bool AddNew(News news, NewsLang newsLang)
        {
            using (var db = new NailShopEntities())
            {
                using (TransactionScope transcope = new TransactionScope())
                {
                    try
                    {
                        db.News.Add(news);
                        db.SaveChanges();
                        newsLang.NewsID = news.NewsID;
                        db.NewsLangs.Add(newsLang);
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

        public bool Edit(News news, NewsLang newsLang)
        {
            using (var db = new NailShopEntities())
            {
                using (TransactionScope transcope = new TransactionScope())
                {
                    try
                    {
                        db.Entry(news).State = System.Data.Entity.EntityState.Modified;
                        db.Entry(newsLang).State = System.Data.Entity.EntityState.Modified;
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
