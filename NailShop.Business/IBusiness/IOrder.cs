using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NailShop.DataAccess;

namespace NailShop.Business
{
    public interface IOrder: IRepository<vw_Invoice>
    {
        List<vw_Invoice> GetOrderByStoreID(long StoreID, DateTime FromDate, DateTime ToDate, int InvoiceStatus);

        List<vw_Invoice> GetData(long CustomerID, DateTime FromDate, DateTime ToDate, int InvoiceStatus);
        Model.ModelWeb.InvoiceModel GetOrderByID(int StoreID, long CustomerID, long OrdID);

        Model.ModelWeb.InvoiceModel GetOrderByID(long StoreID, long OrdID);

        List<GetOrdItem_Result> GetOrdItemByID(int StoreID, long CustomerID, long OrdID);

        List<GetOrdItemByID_Result> GetOrdItemByID(long OrdID);
        string GetRefNo(int StoreID, string RefType);
        bool UpdateRefNo(int StoreID, string RefType);
        bool Save(Invoice invoice, List<OrdItem> ordItems);
        bool Delete(long ID);

        List<vw_PromotionDetail> GetPromotion(int SiteID, int StoreID);
    }
}
