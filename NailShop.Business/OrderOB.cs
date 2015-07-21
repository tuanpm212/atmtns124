using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NailShop.DataAccess;

namespace NailShop.Business
{
    public interface IOrder: IRepository<Invoice>
    {

    }

    public class OrderOB: RepositoryData<Invoice>, IOrder
    {
        public InvoiceSyn GetDataForSyn(string LoginName, string Password, int StoreID)
        {
            try
            {
                InvoiceSyn _result = new InvoiceSyn(); 
                //---Check login---//
                if (CheckLogin(LoginName, Password))
                {
                    using (var db = new NailShopEntities())
                    {
                        var select = from c in db.Invoices
                                     where c.StoreID == StoreID && c.RecordState != (int)Enum.RecordState.Unchange
                                     select c;
                        _result.Invoice = select.ToList();

                        //---OrderItem---//
                        foreach (Invoice row in select.ToList())
                        {
                            var ordItem = from c in db.OrdItems
                                          where c.InvoiceID == row.InvoiceID
                                          select c;

                            if (ordItem.Count() > 0)
                                _result.OrdItem.AddRange(ordItem.ToList());
                        }
                        return _result;
                    }
                }
                else
                    return null;
            }
            catch
            {
                return null;
            }
        }
    }
}
