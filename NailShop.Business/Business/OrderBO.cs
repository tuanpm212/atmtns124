using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NailShop.DataAccess;
using System.Data.Entity;
using System.Transactions;

namespace NailShop.Business
{
    public class OrderBO: RepositoryData<vw_Invoice>, IOrder
    {
        #region Store
            public List<vw_Invoice> GetOrderByStoreID(long StoreID, DateTime FromDate, DateTime ToDate, int InvoiceStatus)
            {
                try
                {
                    using (var db = new NailShopEntities())
                    {
                        if (InvoiceStatus != -1 && FromDate != null && ToDate != null)
                        {
                            var select = from c in db.vw_Invoice
                                         where c.StoreID == StoreID && c.InvoiceDate >= FromDate && c.InvoiceDate <= ToDate && c.InvoiceStatus == InvoiceStatus
                                         orderby c.InvoiceID descending, c.InvoiceDate descending
                                         select c;
                            if (select.Count() > 0)
                                return select.ToList();
                        }
                        else
                        {
                            if (FromDate != null && ToDate != null)
                            {
                                var select = from c in db.vw_Invoice
                                             where c.StoreID == StoreID && c.InvoiceDate >= FromDate && c.InvoiceDate <= ToDate
                                             orderby c.InvoiceID descending, c.InvoiceDate descending
                                             select c;
                                if (select.Count() > 0)
                                    return select.ToList();
                            }
                            else
                            {
                                if (InvoiceStatus != -1)
                                {
                                    var select = from c in db.vw_Invoice
                                                 where c.StoreID == StoreID && c.InvoiceStatus == InvoiceStatus
                                                 orderby c.InvoiceID descending, c.InvoiceDate descending
                                                 select c;
                                    if (select.Count() > 0)
                                        return select.ToList();
                                }
                                else
                                {
                                    var select = from c in db.vw_Invoice
                                                 where c.StoreID == StoreID 
                                                 orderby c.InvoiceID descending, c.InvoiceDate descending
                                                 select c;
                                    if (select.Count() > 0)
                                        return select.ToList();
                                }
                            }
                        }
                        return null;  
                    }
                }
                catch
                {
                    return null;
                }
            }

            public Model.ModelWeb.InvoiceModel GetOrderByID(long StoreID, long OrdID)
            {
                try
                {
                    Model.ModelWeb.InvoiceModel model = new Model.ModelWeb.InvoiceModel();
                    using (var db = new NailShopEntities())
                    {
                        var select = from c in db.Invoices
                                     where c.InvoiceID == OrdID && c.StoreID == StoreID
                                     select c;
                        if (select.Count() > 0)
                        {
                            model.Invoice = select.First();
                            return model;
                        }
                        return null;
                    }
                }
                catch
                {
                    return null;
                }
            }

            public List<GetOrdItemByID_Result> GetOrdItemByID(long OrdID)
            {
                try
                {
                    using (var db = new NailShopEntities())
                    {
                        var select = from c in db.GetOrdItemByID(OrdID)
                                     orderby c.ProductName
                                     select c;
                        if (select.Count() > 0)
                            return select.ToList();
                        return null;
                    }
                }
                catch
                {
                    return null;
                }
            }

        #endregion 

        #region Customer

            public List<vw_Invoice> GetData(long CustomerID, DateTime FromDate, DateTime ToDate, int InvoiceStatus)
            {
                try
                {
                    using (var db = new NailShopEntities())
                    {
                        if (InvoiceStatus != -1 && FromDate != null && ToDate != null)
                        {
                            var select = from c in db.vw_Invoice
                                         where c.CustomerID == CustomerID && c.InvoiceDate >= FromDate && c.InvoiceDate <= ToDate && c.InvoiceStatus == InvoiceStatus
                                         orderby c.InvoiceID descending, c.InvoiceDate descending
                                         select c;
                            if (select.Count() > 0)
                                return select.ToList();
                        }
                        else
                        {
                            if (FromDate != null && ToDate != null)
                            {
                                var select = from c in db.vw_Invoice
                                             where c.CustomerID == CustomerID && c.InvoiceDate >= FromDate && c.InvoiceDate <= ToDate
                                             orderby c.InvoiceID descending, c.InvoiceDate descending
                                             select c;
                                if (select.Count() > 0)
                                    return select.ToList();
                            }
                            else
                            {
                                if(InvoiceStatus!=-1)
                                {
                                    var select = from c in db.vw_Invoice
                                                 where c.CustomerID == CustomerID && c.InvoiceStatus == InvoiceStatus
                                                 orderby c.InvoiceID descending, c.InvoiceDate descending
                                                 select c;
                                    if (select.Count() > 0)
                                        return select.ToList();
                                }
                                else
                                {
                                    var select = from c in db.vw_Invoice
                                                 where c.CustomerID == CustomerID
                                                 orderby c.InvoiceID descending, c.InvoiceDate descending
                                                 select c;
                                    if (select.Count() > 0)
                                        return select.ToList();
                                }
                            }
                        }
                        return null;
                    }
                }
                catch
                {
                    return null;
                }
            }

            public Model.ModelWeb.InvoiceModel GetOrderByID(int StoreID, long CustomerID, long OrdID)
            {
                try
                {
                    Model.ModelWeb.InvoiceModel model = new Model.ModelWeb.InvoiceModel();
                    if (OrdID != -1)
                        return GetOrderID(StoreID, CustomerID, OrdID);
                    else
                    {
                        using (var db = new NailShopEntities())
                        {
                            var select = from c in db.Invoices
                                         where c.StoreID == StoreID && c.CustomerID == CustomerID
                                         orderby c.IsTemplate descending
                                         select c;
                            if (select.Count() > 0)
                                return GetOrderID(StoreID, CustomerID, select.First().InvoiceID);
                            else
                                return GetOrderID(StoreID, CustomerID, -1);
                        }
                    }
                }
                catch
                {
                    return null;
                }
            }

            public List<GetOrdItem_Result> GetOrdItemByID(int StoreID, long CustomerID, long OrdID)
            {
                try
                {
                    if (OrdID != -1)
                        return GetOrdItems(StoreID, CustomerID, OrdID);
                    else
                    {
                        using (var db = new NailShopEntities())
                        {
                            var select = from c in db.Invoices
                                         where c.StoreID == StoreID && c.CustomerID == CustomerID
                                         orderby c.IsTemplate descending
                                         select c;
                            if (select.Count() > 0)
                                return GetOrdItems(StoreID, CustomerID, select.First().InvoiceID);
                            else
                                return GetOrdItems(StoreID, CustomerID, -1);
                        }
                    }
                }
                catch
                {
                    return null;
                }
            }

            public bool Save(Invoice invoice, List<OrdItem> ordItems)
            {
                try
                {
                    using (var db = new NailShopEntities())
                    {
                        using (TransactionScope transcope = new TransactionScope())
                        {
                            try
                            {
                                //---Add New Data---//
                                if (invoice.InvoiceID == -1)
                                {
                                    invoice.InvoiceNo = GetRefNo(invoice.StoreID, "INVOICE");
                                    invoice.InvoiceDate = DateTime.Now;
                                    invoice.LocalID = -1;
                                    invoice.RecordState = (int)Enum.RecordState.AddNew;
                                    db.Invoices.Add(invoice);
                                    db.SaveChanges();
                                    UpdateRefNo(invoice.StoreID, "INVOICE");
                                }
                                else //Update invoice
                                {
                                    var oldInvoie = from c in db.Invoices
                                                    where c.InvoiceID == invoice.InvoiceID
                                                    select c;
                                    if (oldInvoie.Count() > 0)
                                    {
                                        oldInvoie.First().SubTotal = invoice.SubTotal;
                                        oldInvoie.First().Address = invoice.Address;
                                        oldInvoie.First().AddressAddr1S = invoice.AddressAddr1S;
                                        oldInvoie.First().City = invoice.City;
                                        oldInvoie.First().State = invoice.State;
                                        oldInvoie.First().ZipCode = invoice.ZipCode;


                                        oldInvoie.First().CityShip = invoice.CityShip;
                                        oldInvoie.First().StateShip = invoice.StateShip;
                                        oldInvoie.First().ZipCodeShip = invoice.ZipCodeShip;

                                        oldInvoie.First().Notes = invoice.Notes;
                                        oldInvoie.First().SubTotal = invoice.SubTotal;
                                        oldInvoie.First().SaleTax = invoice.SaleTax;
                                        oldInvoie.First().Discount = invoice.Discount;
                                        oldInvoie.First().Total = invoice.Total;
                                        oldInvoie.First().IsTemplate = invoice.IsTemplate;
                                        if (oldInvoie.First().RecordState == (int)Enum.RecordState.Unchange)
                                            oldInvoie.First().RecordState = (int)Enum.RecordState.Modify;
                                        if(oldInvoie.First().InvoiceStatus!=0)
                                            oldInvoie.First().InvoiceStatus = (int)Enum.RecordState.Modify;
                                        db.Entry(oldInvoie.First()).State = EntityState.Modified;

                                        var select = from c in db.OrdItems
                                                     where c.InvoiceID == invoice.InvoiceID
                                                     select c;
                                        foreach (OrdItem row in select.ToList())
                                        {
                                            db.Entry(row).State = EntityState.Deleted;
                                        }
                                        db.SaveChanges();
                                    }
                                    else
                                    {
                                        transcope.Dispose();
                                        return false;
                                    }
                                }

                                foreach (OrdItem row in ordItems)
                                {
                                    row.InvoiceID = invoice.InvoiceID;
                                }
                                db.OrdItems.AddRange(ordItems);

                                //---Update Order template---//
                                if(invoice.IsTemplate)
                                {
                                    var selectTemplate = from c in db.Invoices
                                                         where c.StoreID == invoice.StoreID && c.CustomerID == invoice.CustomerID && c.IsTemplate && c.InvoiceID != invoice.InvoiceID
                                                         select c;
                                    if(selectTemplate.Count()>0)
                                    {
                                        foreach(Invoice mTemp in selectTemplate)
                                        {
                                            mTemp.IsTemplate = false;
                                            db.Entry(mTemp).State = EntityState.Modified;
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
                catch
                {
                    return false;
                }
            }
            private Model.ModelWeb.InvoiceModel GetOrderID(int StoreID, long CustomerID, long OrdID)
            {
                try
                {
                    using (var db = new NailShopEntities())
                    {
                        Model.ModelWeb.InvoiceModel model = new Model.ModelWeb.InvoiceModel();
                        //---Get Customer---//
                        var customer = from c in db.Customers
                                       where c.CustomerID == CustomerID
                                       select c;
                        if (customer.Count() == 0)
                            return null;
                        model.Customer = customer.First();

                        //---Get Order Master---//
                        var invoice = from c in db.Invoices
                                      where c.InvoiceID == OrdID && c.CustomerID == CustomerID
                                      select c;
                        if (invoice.Count() == 1)
                            model.Invoice = invoice.First();
                        return model;
                    }
                }
                catch
                {
                    return null;
                }
            }

            private List<GetOrdItem_Result> GetOrdItems(int StoreID, long CustomerID, long OrdID)
            {
                try
                {
                    using (var db = new NailShopEntities())
                    {
                        var select = from c in db.GetOrdItem(StoreID, CustomerID, OrdID)
                                     orderby c.ProductName
                                     select c;
                        if (select.Count() > 0)
                            return select.ToList();
                        else

                        return null;
                    }
                }
                catch
                {
                    return null;
                }
            }

            public string GetRefNo(int StoreID, string RefType)
            {
                using (var context = new NailShopEntities())
                {
                    var select = from c in context.RefNoes
                                 where c.StoreID == StoreID && c.RefType == RefType
                                 select c;

                    if (select.Count() == 1)
                    {
                        string Result, RefNo, mMonth, mYear;
                        decimal Lenght, CurrNumber;
                        Result = select.First().FormatString;
                        CurrNumber = select.First().CurrNumber + 1;
                        Lenght = select.First().Length;
                        RefNo = CurrNumber.ToString();

                        for (int i = 0; i < Lenght; i++)
                        {
                            if (RefNo.Length == Lenght)
                                break;
                            RefNo = "0" + RefNo;
                        }

                        if (DateTime.Today.Month < 10)
                            mMonth = "0" + DateTime.Today.Month.ToString();
                        else
                            mMonth = DateTime.Today.Month.ToString();
                        mYear = DateTime.Today.Year.ToString().Substring(2, 2);

                        Result = Result.Replace("[n]", RefNo).Replace("[mm]", mMonth).Replace("[yy]", mYear);
                        return Result;
                    }
                    return null;
                }
            }

            public bool UpdateRefNo(int StoreID, string RefType)
            {
                using (var context = new NailShopEntities())
                {
                    var select = from c in context.RefNoes
                                 where c.StoreID == StoreID && c.RefType == RefType
                                 select c;
                    if (select.Count() == 1)
                    {
                        select.First().CurrNumber = select.First().CurrNumber + 1;
                        context.Entry(select.First()).State = EntityState.Modified;
                        context.SaveChanges();
                        return true;
                    }
                    return false;
                }
            }

            public bool Delete(long ID)
            {
                using (var context = new NailShopEntities())
                {
                    var select = from c in context.Invoices
                                 where c.InvoiceID ==ID
                                 select c;
                    if (select.Count() == 1)
                    {
                        context.Entry(select.First()).State = EntityState.Deleted;
                        context.SaveChanges();
                        return true;
                    }
                    return false;
                }
            }

            public List<vw_PromotionDetail> GetPromotion(int SiteID, int StoreID)
            {
                using (var context = new NailShopEntities())
                {
                    var select = from c in context.vw_PromotionDetail
                                 where c.SiteID == SiteID && c.StoreID == StoreID && c.FromDate <= DateTime.Now && c.ToDate >= DateTime.Now
                                 orderby c.Sort descending
                                 select c;
                    if (select.Count()>0)
                        return select.ToList();
                    return null;
                }
            }
        #endregion

    }
}
