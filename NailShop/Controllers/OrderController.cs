using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NailShop.Business;
using NailShop.DataAccess;
using System.Web.Script.Serialization;

namespace NailShop.Controllers
{
    public class OrderController : BaseController
    {
        #region Action Methods
            public ActionResult Index()
            {
                GetInvoiceStatus();
                if (_session.IsLogin && !_session.IsStore)
                {
                    DateTime mFromDate, mToDate;
                    mToDate = DateTime.Now;
                    if (mToDate.Month > 3)
                        mFromDate = new DateTime(mToDate.Year, mToDate.Month - 3, 1);
                    else
                    {
                        int Month;
                        switch (mToDate.Month)
                        {
                            case 1:
                                Month = 10;
                                break;
                            case 2:
                                Month = 11;
                                break;
                            default:
                                Month = 12;
                                break;
                        }
                        mFromDate = new DateTime(mToDate.Year - 1, Month, 1);
                    }
                    ViewBag.FromDate = Utils.Utils.FormatDate(mFromDate);
                    ViewBag.ToDate = Utils.Utils.FormatDate(mToDate);
                    return View();
                }
                else
                    return RedirectToAction("Index", "Home");
            }

            public ActionResult Create(long ID)
            {
                
                if (_session.IsLogin && !_session.IsStore)
                {
                    IOrder _ord = new OrderBO();
                    Business.Model.ModelWeb.InvoiceModel model = new Business.Model.ModelWeb.InvoiceModel();
                    model = _ord.GetOrderByID(_session.StoreID, _session.CustomerID, ID);
                    if (ID==-1)
                    {
                        ViewBag.IsNew = true;
                        ViewBag.InvoiceDate = Utils.Utils.FormatDate(DateTime.Now);
                        if (model.Invoice != null)
                        {
                            ViewBag.BillAddress = model.Invoice.Address;
                            ViewBag.DeliveryAddress = model.Invoice.AddressAddr1S;
                            ViewBag.City = model.Invoice.City ?? model.Customer.City;
                            ViewBag.CityShip = model.Invoice.CityShip ?? model.Customer.City;
                            ViewBag.ZipCode = model.Invoice.ZipCode ?? model.Customer.ZipCode;
                            ViewBag.ZipCodeShip = model.Invoice.ZipCodeShip ?? model.Customer.ZipCode;
                            ViewBag.State = model.Invoice.State ?? model.Customer.State;
                            ViewBag.StateShip = model.Invoice.StateShip ?? model.Customer.State;
                        }
                        else
                        {
                            ViewBag.BillAddress = model.Customer.Address;
                            ViewBag.DeliveryAddress = model.Customer.AddressAddr1S;
                            ViewBag.City = model.Customer.City;
                            ViewBag.CityShip = model.Customer.City;
                            ViewBag.ZipCode = model.Customer.ZipCode;
                            ViewBag.ZipCodeShip = model.Customer.ZipCode;
                            ViewBag.State = model.Customer.State;
                            ViewBag.StateShip = model.Customer.State;
                        }

                        ViewBag.InvoiceNo = _ord.GetRefNo(_session.StoreID, "INVOICE");
                        ViewBag.Amount = "0";
                        ViewBag.DiscountAmount = "0";
                        ViewBag.TaxAmount = "0";
                        ViewBag.TotalAmount = "0";
                        ViewBag.Description = "";
                        ViewBag.IsTemplate = false;
                    }
                    else
                    {
                        if (model.Invoice.InvoiceStatus == 0)
                            ViewBag.IsEdit = true;
                        else
                            ViewBag.IsEdit = false;

                        ViewBag.IsNew = false;
                        ViewBag.BillAddress = model.Invoice.Address;
                        ViewBag.DeliveryAddress = model.Invoice.AddressAddr1S;
                        ViewBag.Description = model.Invoice.Notes;
                        ViewBag.InvoiceDate = Utils.Utils.FormatDate(model.Invoice.InvoiceDate?? DateTime.Now);

                        ViewBag.City = model.Invoice.City;
                        ViewBag.CityShip = model.Invoice.CityShip;
                        ViewBag.ZipCode = model.Invoice.ZipCode;
                        ViewBag.ZipCodeShip = model.Invoice.ZipCodeShip;
                        ViewBag.State = model.Invoice.State;
                        ViewBag.StateShip = model.Invoice.StateShip;

                        ViewBag.InvoiceNo = model.Invoice.InvoiceNo;
                        ViewBag.Amount = model.Invoice.SubTotal.ToString();
                        ViewBag.DiscountAmount = model.Invoice.Discount.ToString();
                        ViewBag.TaxAmount = model.Invoice.SaleTax.ToString();
                        ViewBag.TotalAmount = model.Invoice.Total.ToString();
                        ViewBag.IsTemplate = model.Invoice.IsTemplate;
                    }
                    ViewBag.OrdID = ID;
                    return View();
                }
                else
                    return RedirectToAction("Index", "Home");
            }

        #endregion

        #region Json Data
            public JsonResult GetOrdItem(long ID)
            {
                IOrder _ord = new OrderBO();
                var data = _ord.GetOrdItemByID(_session.StoreID, _session.CustomerID, ID);
                string jsonData = new JavaScriptSerializer().Serialize(data);
                return Json(jsonData, JsonRequestBehavior.AllowGet);
            }
            public JsonResult GetInvoice(DateTime FromDate, DateTime ToDate, int Status)
            {
                if (_session.IsLogin && !_session.IsStore)
                {
                    string jsonData = "[]";
                    IOrder _order = new OrderBO();
                    List<vw_Invoice> data;
                    data = _order.GetData(_session.CustomerID, FromDate, ToDate, Status);
                    if (data != null)
                        jsonData = new JavaScriptSerializer().Serialize(data);
                    return Json(jsonData, JsonRequestBehavior.AllowGet);
                }
                else
                    RedirectToAction("Index", "Home");
                return Json("[]", JsonRequestBehavior.AllowGet);
            }

            public void GetInvoiceStatus()
            {
                InvoiceStatus row;
                List<InvoiceStatus> data = new List<InvoiceStatus>();
                row = new InvoiceStatus();
                row.Status = -1;
                row.Name = "All";
                data.Add(row);

                row = new InvoiceStatus();
                row.Status = 0;
                row.Name = "Working-On";
                data.Add(row);

                row = new InvoiceStatus();
                row.Status = 1;
                row.Name = "Invoiced";
                data.Add(row);

                row = new InvoiceStatus();
                row.Status = 2;
                row.Name = "Refunded";
                data.Add(row);

                row = new InvoiceStatus();
                row.Status = 3;
                row.Name = "Estimated";
                data.Add(row);

                row = new InvoiceStatus();
                row.Status = 4;
                row.Name = "Delivery";
                data.Add(row);

                ViewBag.ListStatus = data;
            }

            [HttpPost]
            public JsonResult SaveData(Invoice invoice, List<GetOrdItem_Result> ordItems)
            {
                try
                {
                    invoice.StoreID = _session.StoreID;
                    invoice.CustomerID = _session.CustomerID;

                    List<OrdItem> ordDtail = new List<OrdItem>();
                    OrdItem mRow;
                    foreach(GetOrdItem_Result row in ordItems)
                    {
                        if (row.Qty > 0)
                        {
                            mRow = new OrdItem();
                            mRow.InvoiceID = invoice.InvoiceID;
                            mRow.ProductID = row.ProductID ?? -1;
                            mRow.ProductName = row.ProductName;
                            mRow.Description = row.Description;
                            mRow.ProdType = row.ProdType;
                            mRow.NoTax = row.NoTax;
                            mRow.Qty = row.Qty;
                            mRow.Price = row.Price;
                            mRow.Discount = mRow.Discount;
                            mRow.Total = row.Total;
                            if (invoice.InvoiceID == -1)
                                mRow.RecordState = (int)NailShop.Business.Enum.RecordState.AddNew;
                            else
                                mRow.RecordState = (int)NailShop.Business.Enum.RecordState.Modify;
                            ordDtail.Add(mRow);
                        }
                    }
                    IOrder _cls = new OrderBO();
                    if(_cls.Save(invoice, ordDtail))
                        return Json(new { IsOk = true }, JsonRequestBehavior.AllowGet);
                    return Json(new { IsOk = false }, JsonRequestBehavior.AllowGet);

                }
                catch
                {
                    return Json(new { IsOk = false }, JsonRequestBehavior.AllowGet);
                }
            }

            [HttpPost]
            public JsonResult Delete(long ID)
            {
                IOrder _ord = new OrderBO();
                var data = _ord.Delete(ID);
                return Json(new { IsOk = data }, JsonRequestBehavior.AllowGet);
            }

            public JsonResult GetPromotionRule()
            {
                string jsonData = "[]";
                IOrder _cls = new OrderBO();
                var data = _cls.GetPromotion(_session.SiteID, _session.StoreID);
                if(data!=null)
                    jsonData = new JavaScriptSerializer().Serialize(data);
                return Json(jsonData, JsonRequestBehavior.AllowGet);
            }

        #endregion
    }
}