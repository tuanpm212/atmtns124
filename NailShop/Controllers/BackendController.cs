using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using NailShop.Business;
using NailShop.DataAccess;
using System.IO;

namespace NailShop.Controllers
{
    public class BackendController : BaseController
    {
        #region ActionResult
            public ActionResult Index()
            {
                //_session.IsLogin = false;
                //_session.IsAdmin = false;
                return View();
            }

        #endregion

        #region Promotion

            public ActionResult Promotion()
            {
                if (_session.IsLogin && _session.IsStore && !_session.IsAdmin)
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
                    return RedirectToAction("index", "backend");
            }

            public ActionResult PromotionCategory()
            {
                if (_session.IsLogin && _session.IsStore)
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
                    return RedirectToAction("index", "backend");
            }

            public ActionResult CreatePromotion(long id)
            {
                if (_session.IsLogin && _session.IsStore && !_session.IsAdmin)
                {
                    DateTime mFromDate, mToDate;
                    mFromDate = DateTime.Now;
                    mToDate = DateTime.Now.AddDays(7);
                    ViewBag.FromDate = Utils.Utils.FormatDate(mFromDate);
                    ViewBag.ToDate = Utils.Utils.FormatDate(mToDate);
                    ViewBag.PromotoinID = id;
                    ViewBag.StoreID = _session.StoreID;
                    return View();
                }
                else
                    return RedirectToAction("index", "backend");
            }

            public ActionResult CreatePromotionCategory(long id)
            {
                if (_session.IsLogin && _session.IsStore && !_session.IsAdmin)
                {
                    DateTime mFromDate, mToDate;
                    mFromDate = DateTime.Now;
                    mToDate = DateTime.Now.AddDays(7);
                    ViewBag.FromDate = Utils.Utils.FormatDate(mFromDate);
                    ViewBag.ToDate = Utils.Utils.FormatDate(mToDate);
                    ViewBag.PromotoinID = id;
                    ViewBag.StoreID = _session.StoreID;
                    return View();
                }
                else
                    return RedirectToAction("Index", "Home");
            }
        
        #endregion

        #region Order
            public ActionResult Order()
            {
                GetInvoiceStatus();
                if (_session.IsLogin && _session.IsStore && !_session.IsAdmin)
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
                    return RedirectToAction("index", "backend");
            }

            public ActionResult CreateOrder(long id)
            {
                if (_session.IsLogin && _session.IsStore && !_session.IsAdmin)
                {
                    IOrder _ord = new OrderBO();
                    Business.Model.ModelWeb.InvoiceModel model = new Business.Model.ModelWeb.InvoiceModel();
                    model = _ord.GetOrderByID(_session.StoreID, id);
                    ViewBag.BillAddress = model.Invoice.Address;
                    ViewBag.DeliveryAddress = model.Invoice.AddressAddr1S;
                    ViewBag.Description = model.Invoice.Notes;
                    ViewBag.InvoiceDate = Utils.Utils.FormatDate(model.Invoice.InvoiceDate ?? DateTime.Now);

                    ViewBag.City = model.Invoice.City;
                    ViewBag.CityShip = model.Invoice.CityShip;
                    ViewBag.ZipCode = model.Invoice.ZipCode;
                    ViewBag.ZipCodeShip = model.Invoice.ZipCodeShip;
                    ViewBag.State = model.Invoice.State;
                    ViewBag.StateShip = model.Invoice.StateShip;

                    ViewBag.InvoiceNo = model.Invoice.InvoiceNo;
                    ViewBag.Amount = string.Format("{0:#,###0}", model.Invoice.SubTotal);
                    ViewBag.DiscountAmount = string.Format("{0:#,###0}", model.Invoice.Discount);
                    ViewBag.TaxAmount = string.Format("{0:#,###0}", model.Invoice.SaleTax);
                    ViewBag.TotalAmount = string.Format("{0:#,###0}", model.Invoice.Total);
                    ViewBag.IsTemplate = model.Invoice.IsTemplate;

                    ViewBag.OrdID = id;
                    return View();
                }
                else
                    return RedirectToAction("index", "backend");
            }
        #endregion

        #region Json Result

            [HttpPost]
            public JsonResult Login(Business.Model.ModelWeb.LoginModel model)
            {
                IStore _cls = new StoreBO();
                Store _store = _cls.Login(model.LoginName, model.Password);
                if (_store != null && !_store.IsAdmin)
                {
                    _session.IsLogin = true;
                    _session.IsStore = true;
                    _session.CustomerID = -1;
                    _session.FullName = _store.StoreName;
                    _session.StoreID = _store.StoreID;
                    _session.IsAdmin = false;
                    return Json(new { IsOk = true }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    _session.IsLogin = false;
                    _session.IsStore = false;
                    _session.CustomerID = -1;
                    _session.IsAdmin = false;
                    return Json(new { IsOk = false }, JsonRequestBehavior.AllowGet);
                }
            }

            [HttpPost]
            public JsonResult Logout()
            {
                if (_session.IsLogin)
                {
                    _session.IsLogin = false;
                    _session.IsAdmin = false;
                    _session.IsStore = false;
                    _session.StoreID = -1;
                    _session.FullName = null;
                    return Json(new { IsOk = true }, JsonRequestBehavior.AllowGet);
                }
                else
                    return Json(new { IsOk = false }, JsonRequestBehavior.AllowGet);
            }

            [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
            public JsonResult GetPromotion(DateTime FromDate, DateTime ToDate, bool IsItem)
            {
                if (_session.IsLogin && _session.IsStore && !_session.IsAdmin)
                {
                    string jsonData = "[]";
                    IPromotion _cls = new PromotionBO();
                    var data = _cls.GetPromotion(_session.SiteID, _session.StoreID, FromDate, ToDate, IsItem);
                    if (data != null)
                        jsonData = new JavaScriptSerializer().Serialize(data);
                    return Json(jsonData, JsonRequestBehavior.AllowGet);
                }
                else
                    RedirectToAction("index", "backend");
                return Json("[]", JsonRequestBehavior.AllowGet);
            }

            [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
            public JsonResult GetPromotionItem(long id)
            {
                if (_session.IsLogin && _session.IsStore && !_session.IsAdmin)
                {
                    if (id == -1)
                    {
                        string jsonData = "";
                        jsonData = "{\"master\":" + new JavaScriptSerializer().Serialize(new Promotion());
                        jsonData += ",\"detail\":[]}";
                        return Json(jsonData, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        string jsonData = "";
                        IPromotion _cls = new PromotionBO();
                        var data = _cls.GetPromotionItem(id);
                        jsonData = "{\"master\":" + new JavaScriptSerializer().Serialize(data[0]);
                        jsonData += ",\"detail\":" + new JavaScriptSerializer().Serialize(data[1]) + "}"; ;
                        return Json(jsonData, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                    RedirectToAction("index", "backend");
                return Json("[]", JsonRequestBehavior.AllowGet);
            }

            [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
            public JsonResult GetOrdForBackend(long ID)
            {
                string jsonData = "[]";
                IOrder _ord = new OrderBO();
                var data = _ord.GetOrdItemByID(ID);
                if (data != null)
                    jsonData = new JavaScriptSerializer().Serialize(data);
                return Json(jsonData, JsonRequestBehavior.AllowGet);
            }

            [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
            public JsonResult GetOrderByStoreID(DateTime FromDate, DateTime ToDate, int Status)
            {
                if (_session.IsLogin && _session.IsStore && !_session.IsAdmin)
                {
                    string jsonData = "[]";
                    IOrder _order = new OrderBO();
                    List<vw_Invoice> data;
                    data = _order.GetOrderByStoreID(_session.StoreID, FromDate, ToDate, Status);
                    if (data != null)
                        jsonData = new JavaScriptSerializer().Serialize(data);
                    return Json(jsonData, JsonRequestBehavior.AllowGet);
                }
                else
                    RedirectToAction("index", "backend");
                return Json("[]", JsonRequestBehavior.AllowGet);
            }

            [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
            public JsonResult GetItemForPromotion(int StoreID, string TextSearch)
            {
                if (_session.IsLogin && _session.IsStore && !_session.IsAdmin)
                {
                    string jsonData = "[]";
                    IPromotion _cls = new PromotionBO();
                    var data = _cls.GetItemForPromotion(StoreID, TextSearch);
                    if (data != null)
                        jsonData = new JavaScriptSerializer().Serialize(data);
                    return Json(jsonData, JsonRequestBehavior.AllowGet);
                }
                else
                    RedirectToAction("index", "backend");
                return Json("[]", JsonRequestBehavior.AllowGet);
            }

            [HttpPost]
            public JsonResult SavePromotion(Promotion master, List<PromotionDetail> detail)
            {
                if (_session.IsLogin && _session.IsStore && !_session.IsAdmin)
                {
                    IPromotion _cls = new PromotionBO();
                    var IsResult = _cls.SavePromotion(master, detail);
                    return Json(new { IsOk = IsResult }, JsonRequestBehavior.AllowGet);
                }
                else
                    RedirectToAction("index", "backend");
                return Json("[]", JsonRequestBehavior.AllowGet);
            }

            [HttpPost]
            public JsonResult DeletePromotion(int StoreID, long ID)
            {
                if (_session.IsLogin && _session.IsStore && !_session.IsAdmin)
                {
                    if (_session.StoreID == StoreID)
                    {
                        IPromotion _cls = new PromotionBO();
                        var IsResult = _cls.Delete(ID);
                        return Json(new { IsOk = IsResult }, JsonRequestBehavior.AllowGet);
                    }
                    else
                        return Json(new { IsOk = false }, JsonRequestBehavior.AllowGet);
                }
                else
                    RedirectToAction("index", "backend");
                return Json("[]", JsonRequestBehavior.AllowGet);
            }

            [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
            public JsonResult GetPromotionCategory(long id)
            {
                if (_session.IsLogin && _session.IsStore && !_session.IsAdmin)
                {
                    if (id == -1)
                    {
                        string jsonData = "";
                        jsonData = "{\"master\":" + new JavaScriptSerializer().Serialize(new Promotion());
                        jsonData += ",\"detail\":[]}";
                        return Json(jsonData, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        string jsonData = "";
                        IPromotion _cls = new PromotionBO();
                        var data = _cls.GetPromotionCategory(id);
                        jsonData = "{\"master\":" + new JavaScriptSerializer().Serialize(data[0]);
                        jsonData += ",\"detail\":" + new JavaScriptSerializer().Serialize(data[1]) + "}";
                        return Json(jsonData, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                    RedirectToAction("index", "backend");
                return Json("[]", JsonRequestBehavior.AllowGet);
            }

            [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
            public JsonResult GetCategoryForPromotion(int StoreID, string TextSearch)
            {
                if (_session.IsLogin && _session.IsStore && !_session.IsAdmin)
                {
                    string jsonData = "[]";
                    IPromotion _cls = new PromotionBO();
                    var data = _cls.GetCategoryForPromotion(StoreID, TextSearch);
                    if (data != null)
                        jsonData = new JavaScriptSerializer().Serialize(data);
                    return Json(jsonData, JsonRequestBehavior.AllowGet);
                }
                else
                    RedirectToAction("index", "backend");
                return Json("[]", JsonRequestBehavior.AllowGet);
            }

        #endregion

        #region Private Method
            private void GetInvoiceStatus()
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

        #endregion
    }

    public class InvoiceStatus
    {
        public int Status { get; set; }
        public string Name { get; set; }
    }
}