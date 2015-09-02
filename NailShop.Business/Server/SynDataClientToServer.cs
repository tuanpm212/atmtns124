using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using NailShop.DataAccess;

namespace NailShop.Business
{
    public class SynDataClientToServer: SynBase
    {
        #region Syn data

            public bool SynProductToServer(string LoginName, string Password, int StoreID, List<NailSyn.DataAccess.SynProductClient> ListProduct)
            {
                try
                {
                    if (this.CheckLogin(LoginName, Password))
                    {
                        using (var db = new NailShopEntities())
                        {
                            using (TransactionScope transcope = new TransactionScope())
                            {
                                try
                                {
                                    Product _row;
                                    foreach (NailSyn.DataAccess.SynProductClient row in ListProduct)
                                    {
                                        var select = from c in db.Products
                                                     where c.StoreID == StoreID && c.LocalID == row.ProductID
                                                     select c;
                                        //Add new row
                                        if (select.Count() == 0)
                                        {
                                            if (row.RecordState == (int)Enum.RecordState.AddNew)
                                            {
                                                _row = new Product();
                                                _row.StoreID = StoreID;
                                                _row.LocalID = row.ProductID;
                                                _row.BarCode = row.BarCode;
                                                _row.ProdNo = row.ProdNo;
                                                _row.ProductName = row.ProductName;
                                                _row.Description = row.Description;
                                                _row.Price = row.Price;
                                                _row.Price1 = row.Price1;
                                                _row.Price2 = row.Price2;
                                                _row.Price3 = row.Price3;
                                                _row.ProdType = row.ProdType;
                                                _row.ProductUrl = row.ProductUrl;
                                                _row.UnitsInStock = row.UnitsInStock;
                                                _row.InItem = row.InItem;
                                                _row.NoTax = row.NoTax;
                                                _row.EditSequence = row.EditSequence;
                                                _row.RecordState = (int)Enum.RecordState.Unchange;
                                                db.Products.Add(_row);
                                            }
                                        }
                                        else
                                        {
                                            //Edit row data
                                            if (row.RecordState == (int)Enum.RecordState.Modify)
                                            {
                                                select.First().BarCode = row.BarCode;
                                                select.First().ProdNo = row.ProdNo;
                                                select.First().ProductName = row.ProductName;
                                                select.First().Description = row.Description;
                                                select.First().Price = row.Price;
                                                select.First().Price1 = row.Price1;
                                                select.First().Price2 = row.Price2;
                                                select.First().Price3 = row.Price3;
                                                select.First().ProdType = row.ProdType;
                                                select.First().ProductUrl = row.ProductUrl;
                                                select.First().UnitsInStock = row.UnitsInStock;
                                                select.First().InItem = row.InItem;
                                                select.First().NoTax = row.NoTax;
                                                select.First().EditSequence = row.EditSequence;
                                                select.First().RecordState = (int)Enum.RecordState.Unchange;
                                                db.Entry(select.First()).State = System.Data.Entity.EntityState.Modified;
                                            }
                                            else //Delete row data
                                                db.Entry(select.First()).State = System.Data.Entity.EntityState.Deleted;
                                        }
                                    }
                                    db.SaveChanges();
                                    transcope.Complete();
                                    return true;
                                }
                                catch
                                {
                                    transcope.Dispose();
                                }
                            }
                            return false;
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

            public bool SynCustomerToServer(string LoginName, string Password, int StoreID, List<NailSyn.DataAccess.SynCustomerClient> ListCustomer)
            {
                try
                {
                    if (this.CheckLogin(LoginName, Password))
                    {
                        using (var db = new NailShopEntities())
                        {
                            using (TransactionScope transcope = new TransactionScope())
                            {
                                try
                                {
                                    Customer _row;
                                    foreach (NailSyn.DataAccess.SynCustomerClient row in ListCustomer)
                                    {
                                        var select = from c in db.Customers
                                                     where c.StoreID == StoreID && c.LocalID == row.CustomerID
                                                     select c;

                                        //Add new row data
                                        if (select.Count() == 0)
                                        {
                                            if (row.RecordState == (int)Enum.RecordState.AddNew)
                                            {
                                                _row = new Customer();
                                                _row.StoreID = StoreID;
                                                _row.LocalID = row.CustomerID;
                                                _row.LoginName = row.LoginName;
                                                _row.Password = row.Password;
                                                _row.Active = row.Active;
                                                _row.BarCode = row.BarCode;
                                                _row.FullName = row.FullName;
                                                _row.FirstName = row.FirstName;
                                                _row.MiddleName = row.MiddleName;
                                                _row.LastName = row.LastName;
                                                _row.Address = row.Address;
                                                _row.City = row.City;
                                                _row.State = row.State;
                                                _row.ZipCode = row.ZipCode;
                                                _row.AddressAddr1 = row.AddressAddr1;
                                                _row.AddressAddr2 = row.AddressAddr2;
                                                _row.AddressAddr3 = row.AddressAddr3;
                                                _row.AddressAddr4 = row.AddressAddr4;
                                                _row.AddressAddr5 = row.AddressAddr5;
                                                _row.AddressAddr1S = row.AddressAddr1S;
                                                _row.AddressAddr2S = row.AddressAddr2S;
                                                _row.AddressAddr3S = row.AddressAddr3S;
                                                _row.AddressAddr4S = row.AddressAddr4S;
                                                _row.AddressAddr5S = row.AddressAddr5S;
                                                _row.International = row.International;
                                                _row.Country = row.Country;
                                                _row.HomePhone = row.HomePhone;
                                                _row.CellPhone = row.CellPhone;
                                                _row.Notes = row.Notes;
                                                _row.Email = row.Email;
                                                _row.BirthDay = row.BirthDay;
                                                _row.PriceLevel = row.PriceLevel;
                                                _row.TotalSpend = row.TotalSpend;
                                                _row.CustSince = row.CustSince;
                                                _row.LastVisit = row.LastVisit;
                                                _row.NumVisit = row.NumVisit;
                                                _row.RecordState = (int)Enum.RecordState.Unchange;
                                                db.Customers.Add(_row);
                                            }
                                        }
                                        else
                                        {
                                            //Edit row data
                                            if (row.RecordState == (int)Enum.RecordState.Modify)
                                            {
                                                select.First().Active = row.Active;
                                                select.First().BarCode = row.BarCode;
                                                select.First().FullName = row.FullName;
                                                select.First().FirstName = row.FirstName;
                                                select.First().MiddleName = row.MiddleName;
                                                select.First().LastName = row.LastName;
                                                select.First().Address = row.Address;
                                                select.First().City = row.City;
                                                select.First().State = row.State;
                                                select.First().ZipCode = row.ZipCode;
                                                select.First().AddressAddr1 = row.AddressAddr1;
                                                select.First().AddressAddr2 = row.AddressAddr2;
                                                select.First().AddressAddr3 = row.AddressAddr3;
                                                select.First().AddressAddr4 = row.AddressAddr4;
                                                select.First().AddressAddr5 = row.AddressAddr5;
                                                select.First().AddressAddr1S = row.AddressAddr1S;
                                                select.First().AddressAddr2S = row.AddressAddr2S;
                                                select.First().AddressAddr3S = row.AddressAddr3S;
                                                select.First().AddressAddr4S = row.AddressAddr4S;
                                                select.First().AddressAddr5S = row.AddressAddr5S;
                                                select.First().International = row.International;
                                                select.First().Country = row.Country;
                                                select.First().HomePhone = row.HomePhone;
                                                select.First().CellPhone = row.CellPhone;
                                                select.First().Notes = row.Notes;
                                                select.First().Email = row.Email;
                                                select.First().BirthDay = row.BirthDay;
                                                select.First().PriceLevel = row.PriceLevel;
                                                select.First().TotalSpend = row.TotalSpend;
                                                select.First().CustSince = row.CustSince;
                                                select.First().LastVisit = row.LastVisit;
                                                select.First().NumVisit = row.NumVisit;
                                                select.First().RecordState = (int)Enum.RecordState.Unchange;
                                                db.Entry(select.First()).State = System.Data.Entity.EntityState.Modified;
                                            }
                                            else//Delete row data
                                            {
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
                                }
                            }
                            return false;
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

            public bool SynInvoiceClientToServer(string LoginName, string Password, int StoreID, NailSyn.DataAccess.Model.InvoiceClient dataInvoice)
            {
                try
                {
                    using (var db = new NailShopEntities())
                    {
                        using (TransactionScope transcope = new TransactionScope())
                        {
                            try
                            {
                                NailShop.DataAccess.Invoice _invoice;
                                foreach (NailSyn.DataAccess.SynInvoiceClient row in dataInvoice.ListInvoice)
                                {
                                    var select = from c in db.Invoices
                                                 where c.StoreID == StoreID && c.LocalID == row.InvoiceID
                                                 select c;
                                    if (select.Count() == 0)
                                    {
                                        if (row.RecordState == (int)Enum.RecordState.AddNew)
                                        {
                                            _invoice = new Invoice();
                                            _invoice.LocalID = row.InvoiceID;
                                            _invoice.StoreID = StoreID;
                                            _invoice.IsPaid = row.IsPaid;
                                            _invoice.IsPending = row.IsPending;
                                            _invoice.InvoiceDate = row.InvoiceDate;
                                            _invoice.InvoiceNo = row.InvoiceNo;
                                            _invoice.BusinessName = row.BusinessName;
                                            _invoice.Address = row.Address;
                                            _invoice.City = row.City;
                                            _invoice.State = row.City;
                                            _invoice.ZipCode = row.ZipCode;
                                            _invoice.AddressAddr1 = row.AddressAddr1;
                                            _invoice.AddressAddr2 = row.AddressAddr2;
                                            _invoice.AddressAddr3 = row.AddressAddr3;
                                            _invoice.AddressAddr4 = row.AddressAddr4;
                                            _invoice.AddressAddr5 = row.AddressAddr5;
                                            _invoice.AddressAddr1S = row.AddressAddr1S;
                                            _invoice.AddressAddr2S = row.AddressAddr2S;
                                            _invoice.AddressAddr3S = row.AddressAddr3S;
                                            _invoice.AddressAddr4S = row.AddressAddr4S;
                                            _invoice.AddressAddr5S = row.AddressAddr5S;

                                            _invoice.SkipTax = row.SkipTax;
                                            _invoice.SubTotal = row.SubTotal;
                                            _invoice.SaleTax = row.SaleTax;
                                            _invoice.Deposit = row.Deposit;
                                            _invoice.AmtPaid = row.AmtPaid;
                                            _invoice.Discount = row.Discount;
                                            _invoice.AmtCredit = row.AmtCredit;
                                            _invoice.Shipping = row.Shipping;
                                            _invoice.Balance = row.Balance;
                                            _invoice.Total = row.Total;
                                            _invoice.Voided = row.Voided;
                                            _invoice.Notes = row.Notes;
                                            _invoice.Memo = row.Memo;
                                            _invoice.RecordState = (int)Enum.RecordState.Unchange;
                                            _invoice.InvoiceStatus = row.InvoiceStatus;
                                            if(row.CustomerID!=null)
                                            {
                                                var customer = from c in db.Customers
                                                               where c.StoreID == StoreID && c.LocalID == row.CustomerID
                                                               select c;
                                                _invoice.CustomerID = customer.First().CustomerID;
                                            }
                                            db.Invoices.Add(_invoice);
                                            db.SaveChanges();

                                            //Insert OrdItem
                                            OrdItem _OrdItem;
                                            foreach (NailSyn.DataAccess.SynOrdItemClient rowDetail in dataInvoice.ListOrdItem)
                                            {
                                                if (rowDetail.InvoiceID == row.InvoiceID)
                                                {
                                                    if (rowDetail.RecordState != (int)Enum.RecordState.Delete)
                                                    {
                                                        _OrdItem = new OrdItem();
                                                        _OrdItem.InvoiceID = _invoice.InvoiceID;
                                                       
                                                        _OrdItem.ProductName = rowDetail.ProductName;
                                                        _OrdItem.Description = rowDetail.Description;
                                                        _OrdItem.ProdType = rowDetail.ProdType;
                                                        _OrdItem.NoTax = rowDetail.NoTax;
                                                        _OrdItem.Qty = rowDetail.Qty;
                                                        _OrdItem.Price = rowDetail.Price;
                                                        _OrdItem.Discount = rowDetail.Discount;
                                                        _OrdItem.Total = rowDetail.Total;
                                                        _OrdItem.RecordState = (int)Enum.RecordState.Unchange;
                                                        var selectProduct = from c in db.Products
                                                                                where c.LocalID == rowDetail.ProductID && c.StoreID == StoreID
                                                                                select c;
                                                        _OrdItem.ProductID = selectProduct.First().ProductID;
                                                        db.OrdItems.Add(_OrdItem);
                                                    }
                                                }
                                            }
                                        }

                                    }
                                    else
                                    {
                                        if (select.Count() == 1)
                                        {
                                            long InvoiceID = select.First().InvoiceID;
                                            var selectInvoice = from c in db.Invoices
                                                                where c.InvoiceID == InvoiceID
                                                                select c;

                                            if (row.RecordState == (int)Enum.RecordState.Modify)
                                            {
                                                if (row.CustomerID != null)
                                                {
                                                    var customer = from c in db.Customers
                                                                   where c.StoreID == StoreID && c.LocalID == row.CustomerID
                                                                   select c;
                                                    selectInvoice.First().CustomerID = customer.First().CustomerID;
                                                }
                                                selectInvoice.First().IsPaid = row.IsPaid;
                                                selectInvoice.First().IsPending = row.IsPending;
                                                selectInvoice.First().InvoiceDate = row.InvoiceDate;
                                                selectInvoice.First().InvoiceNo = row.InvoiceNo;
                                                selectInvoice.First().BusinessName = row.BusinessName;
                                                selectInvoice.First().Address = row.Address;
                                                selectInvoice.First().City = row.City;
                                                selectInvoice.First().State = row.City;
                                                selectInvoice.First().ZipCode = row.ZipCode;
                                                selectInvoice.First().AddressAddr1 = row.AddressAddr1;
                                                selectInvoice.First().AddressAddr2 = row.AddressAddr2;
                                                selectInvoice.First().AddressAddr3 = row.AddressAddr3;
                                                selectInvoice.First().AddressAddr4 = row.AddressAddr4;
                                                selectInvoice.First().AddressAddr5 = row.AddressAddr5;
                                                selectInvoice.First().AddressAddr1S = row.AddressAddr1S;
                                                selectInvoice.First().AddressAddr2S = row.AddressAddr2S;
                                                selectInvoice.First().AddressAddr3S = row.AddressAddr3S;
                                                selectInvoice.First().AddressAddr4S = row.AddressAddr4S;
                                                selectInvoice.First().AddressAddr5S = row.AddressAddr5S;

                                                selectInvoice.First().SkipTax = row.SkipTax;
                                                selectInvoice.First().SubTotal = row.SubTotal;
                                                selectInvoice.First().SaleTax = row.SaleTax;
                                                selectInvoice.First().Deposit = row.Deposit;
                                                selectInvoice.First().AmtPaid = row.AmtPaid;
                                                selectInvoice.First().Discount = row.Discount;
                                                selectInvoice.First().AmtCredit = row.AmtCredit;
                                                selectInvoice.First().Shipping = row.Shipping;
                                                selectInvoice.First().Balance = row.Balance;
                                                selectInvoice.First().Total = row.Total;
                                                selectInvoice.First().Voided = row.Voided;
                                                selectInvoice.First().Notes = row.Notes;
                                                selectInvoice.First().Memo = row.Memo;
                                                selectInvoice.First().RecordState = (int)Enum.RecordState.Unchange;
                                                selectInvoice.First().InvoiceStatus = row.InvoiceStatus;
                                                db.Entry(select.First()).State = System.Data.Entity.EntityState.Modified;

                                                //Clear Detail & insert
                                                var selectDetail = from c in db.OrdItems
                                                                   where c.InvoiceID == InvoiceID
                                                                   select c;
                                                db.OrdItems.RemoveRange(selectDetail.ToList());
                                                db.SaveChanges();

                                                //Insert OrdItem
                                                OrdItem _OrdItem;
                                                foreach (NailSyn.DataAccess.SynOrdItemClient rowDetail in dataInvoice.ListOrdItem)
                                                {
                                                    if (rowDetail.InvoiceID == row.InvoiceID)
                                                    {
                                                        if (rowDetail.RecordState != (int)Enum.RecordState.Delete)
                                                        {
                                                            _OrdItem = new OrdItem();
                                                            _OrdItem.InvoiceID = InvoiceID;
                                                            _OrdItem.ProductName = rowDetail.ProductName;
                                                            _OrdItem.Description = rowDetail.Description;
                                                            _OrdItem.ProdType = rowDetail.ProdType;
                                                            _OrdItem.NoTax = rowDetail.NoTax;
                                                            _OrdItem.Qty = rowDetail.Qty;
                                                            _OrdItem.Price = rowDetail.Price;
                                                            _OrdItem.Discount = rowDetail.Discount;
                                                            _OrdItem.Total = rowDetail.Total;
                                                            _OrdItem.RecordState = (int)Enum.RecordState.Unchange;
                                                            
                                                            var selectProduct = from c in db.Products
                                                                                where c.LocalID == rowDetail.ProductID && c.StoreID == StoreID
                                                                                select c;
                                                            _OrdItem.ProductID = selectProduct.First().ProductID;
                                                            db.OrdItems.Add(_OrdItem);
                                                        }
                                                    }
                                                }
                                                db.SaveChanges();
                                            }
                                            else
                                            {
                                               //Delete Detail
                                                var selectDetail = from c in db.OrdItems
                                                                   where c.InvoiceID == InvoiceID
                                                                   select c;
                                                db.OrdItems.RemoveRange(selectDetail.ToList());
                                                db.Entry(select.First()).State = System.Data.Entity.EntityState.Deleted;
                                                db.SaveChanges();
                                            }
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

        #endregion
    }
}
