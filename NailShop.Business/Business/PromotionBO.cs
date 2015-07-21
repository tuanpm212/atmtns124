using NailShop.DataAccess;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace NailShop.Business
{
    public class PromotionBO : RepositoryData<Promotion>, IPromotion
    {
        public List<Promotion> GetPromotion(int SiteID, long StoreID, DateTime FromDate, DateTime ToDate, bool IsItem)
        {
            using (var context = new NailShopEntities())
            {
                var select = from c in context.Promotions
                             where c.SiteID == SiteID && c.StoreID == StoreID &&
                             ((FromDate <= c.FromDate && c.FromDate <= ToDate) ||
                             (c.FromDate <= FromDate && ToDate <= c.ToDate) ||
                             (c.FromDate <= FromDate && c.ToDate <= ToDate) ||
                             (FromDate <= c.FromDate && c.ToDate <= ToDate))
                             && c.IsItem == IsItem
                             orderby c.PromotionID descending
                             select c;
                if (select.Count() > 0)
                    return select.ToList();
                return null;
            }
        }

        public ArrayList GetPromotionItem(long ID)
        {
            using (var context = new NailShopEntities())
            {
                ArrayList arrResult = new ArrayList();
                var promotion = from c in context.Promotions
                                where c.PromotionID == ID
                                select c;
                arrResult.Add(promotion.First());

                var promotionDetail = from c in context.vw_PromotionItem
                                      where c.PromotionID == ID
                                      orderby c.Sort
                                      select c;
                arrResult.Add(promotionDetail.ToList());
                return arrResult;
            }
        }

        public List<vw_ProductPromotion> GetItemForPromotion(int StoreID, string TextSearch)
        {
            using (var context = new NailShopEntities())
            {
                var select = from c in context.vw_ProductPromotion
                             where c.StoreID == StoreID && (c.ProductName.Contains(TextSearch) || c.ProdNo.Contains(TextSearch))
                             orderby c.ProductName
                             select c;
                if (select.Count() > 0)
                    return select.ToList();
                return null;
            }
        }

        public bool SavePromotion(Promotion master, List<PromotionDetail> detail)
        {
            try
            {
                using (var db = new NailShopEntities())
                {
                    using (TransactionScope transcope = new TransactionScope())
                    {
                        try
                        {
                            if (master.PromotionID == -1)
                            {
                                db.Promotions.Add(master);
                                db.SaveChanges();

                                foreach (PromotionDetail row in detail)
                                {
                                    row.PromotionID = master.PromotionID;
                                }
                                db.PromotionDetails.AddRange(detail);
                                db.SaveChanges();
                                transcope.Complete();
                                return true;
                            }
                            else
                            {
                                //var select = from c in db.PromotionDetails
                                //             where c.PromotionID == master.PromotionID;
                                //             select c;
                                db.Entry(master).State = System.Data.Entity.EntityState.Modified;
                                db.PromotionDetails.RemoveRange(db.PromotionDetails.Where(c => c.PromotionID == master.PromotionID));
                                db.PromotionDetails.AddRange(detail);
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
            catch
            {
                return false;
            }
        }
        
        public bool Delete(long ID)
        {
            try
            {
                using (var db = new NailShopEntities())
                {
                    db.Promotions.RemoveRange(db.Promotions.Where(c => c.PromotionID == ID));
                    db.SaveChanges();
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        public ArrayList GetPromotionCategory(long ID)
        {
            using (var context = new NailShopEntities())
            {
                ArrayList arrResult = new ArrayList();
                var promotion = from c in context.Promotions
                                where c.PromotionID == ID
                                select c;
                arrResult.Add(promotion.First());

                var promotionDetail = from c in context.vw_Category
                                      where c.PromotionID == ID
                                      select c;
                arrResult.Add(promotionDetail.ToList());
                return arrResult;
            }
        }

        public List<Category> GetCategoryForPromotion(int StoreID, string TextSearch)
        {
            using (var db = new NailShopEntities())
            {
                var select = from c in db.Categories
                             where c.StoreID == StoreID && (c.Code.Contains(TextSearch) || c.CategoryName.Contains(TextSearch))
                             select c;
                if (select.Count() > 0)
                    return select.ToList();
                return null;
            }
        }
    }
}
