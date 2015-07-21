using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NailShop.DataAccess;
using System.Transactions;

namespace NailShop.Business
{
    public class SynDataServerToClient:SynBase
    {
        public Model.ModelInvoice GetInvoiceForSyn(string LoginName, string Password, int StoreID)
        {
            try
            {
                if (this.CheckLogin(LoginName, Password))
                {
                    using (var db = new NailShopEntities())
                    {
                        Model.ModelInvoice data = new Model.ModelInvoice();
                        var select = from c in db.SynInvoices
                                     where c.StoreID == StoreID && c.RecordState != (int)Enum.RecordState.Unchange
                                     orderby c.InvoiceDate
                                     select c;

                        List<SynInvoice> listSynInvoice = select.Take(30).ToList();
                        data.listSynInvoice = listSynInvoice;
                        List<SynOrderItem> listOrdItem = new List<SynOrderItem>();
                        
                        foreach (SynInvoice row in listSynInvoice)
                        {
                            var selectOrdItem = from c in db.SynOrderItems
                                                where c.InvoiceID == row.InvoiceID
                                                select c;
                            if(selectOrdItem.Count()>0)
                                listOrdItem.AddRange(selectOrdItem.ToList());
                        }
                        data.listOrdItem = listOrdItem;
                        return data;
                    }
                }
                return null;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Update RecordState of Invoice
        /// </summary>
        /// <param name="LoginName">LoginName</param>
        /// <param name="Password">Password</param>
        /// <param name="StoreID">StoreID</param>
        /// <param name="listInvoice">List of invoice</param>
        /// <returns></returns>
        public bool UpdateInvoiceAfterSyn(string LoginName, string Password, int StoreID, List<NailShop.DataAccess.SynInvoice> listInvoice)
        {
            try
            {
                if (CheckLogin(LoginName, Password))
                {
                    using (var db = new NailShopEntities())
                    {
                        using (TransactionScope transcope = new TransactionScope())
                        {
                            try
                            {
                                foreach (NailShop.DataAccess.SynInvoice row in listInvoice)
                                {
                                    var select = from c in db.Invoices
                                                 where c.InvoiceID == row.InvoiceID && c.StoreID == StoreID
                                                 select c;
                                    if (row.RecordState == (int)Enum.RecordState.AddNew || row.RecordState == (int)Enum.RecordState.Modify)
                                    {
                                        select.First().LocalID = row.LocalID;
                                        select.First().RecordState = (int)Enum.RecordState.Unchange;
                                        db.Entry(select.First()).State = System.Data.Entity.EntityState.Modified;

                                        var selectOrdItem = from c in db.OrdItems
                                                            where c.InvoiceID == row.InvoiceID
                                                            select c;
                                        if (selectOrdItem.Count() > 0)
                                        {
                                            foreach (OrdItem rowOrd in selectOrdItem.ToList())
                                            {
                                                if (rowOrd.RecordState == (int)Enum.RecordState.AddNew || rowOrd.RecordState == (int)Enum.RecordState.Modify)
                                                {
                                                    rowOrd.RecordState = (int)Enum.RecordState.Unchange;
                                                    db.Entry(rowOrd).State = System.Data.Entity.EntityState.Modified;
                                                }
                                                else
                                                {
                                                    db.Entry(rowOrd).State = System.Data.Entity.EntityState.Deleted;
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (row.RecordState == (int)Enum.RecordState.Delete)
                                        {
                                            var selectOrdItem = from c in db.OrdItems
                                                                where c.InvoiceID == row.InvoiceID
                                                                select c;

                                            db.OrdItems.RemoveRange(selectOrdItem.ToList());
                                            db.Entry(select.First()).State = System.Data.Entity.EntityState.Deleted;
                                        }
                                    }
                                }
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
                else
                    return false;
            }
            catch
            {
                return false;
            }
        }

    }
}
