using NailShop.DataAccess;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NailShop.Business
{
    public interface IPromotion : IRepository<Promotion>
    {
        List<Promotion> GetPromotion(int SiteID, long StoreID, DateTime FromDate, DateTime ToDate, bool IsItem);
        ArrayList GetPromotionItem(long ID);
        List<vw_ProductPromotion> GetItemForPromotion(int StoreID, string TextSearch);
        bool SavePromotion(Promotion master, List<PromotionDetail> detail);
        bool Delete(long ID);

        ArrayList GetPromotionCategory(long ID);
        List<Category> GetCategoryForPromotion(int StoreID, string TextSearch);
    }
}
