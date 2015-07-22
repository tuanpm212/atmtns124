using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NailShop.Business;
using NailShop.Business.Model;
using NailShop.DataAccess;
using System.Web.Script.Serialization;
using Newtonsoft.Json;

namespace NailShop.Controllers
{
    public class HomeController : BaseController
    {
        #region ActionResult

            public ActionResult Index()
            {
                if ((_session.IsLogin && _session.IsStore) || (_session.IsLogin && _session.IsAdmin))
                {
                    _session.IsLogin = false;
                    _session.StoreID = -1;
                    _session.FullName = null;
                    return RedirectToAction("Index", "Home");
                }
                IHome iHome = new HomeBO();
                Business.Model.ModelWeb.Home model = new Business.Model.ModelWeb.Home();
                model = iHome.GetHomeData(_session.SiteID, _session.LangID);
                return View(model);
            }

            [HttpGet]
            public ActionResult Contact()
            {
                Contact model = new Contact();
                return View(model);
            }

            [HttpPost]
            public ActionResult Contact(NailShop.DataAccess.Contact model)
            {
                IContact _contact = new ContactBO();
                if (model != null)
                {
                    model.CreatedDate = DateTime.Now;
                    model.SiteID = _session.SiteID;
                    if (_contact.Add(model))
                    {
                        string body = string.Format(Resources.EmailTemplate.feedbackBody, model.UserName);
                        this.SendEmailMessage(model.Email, Resources.EmailTemplate.feedbackSubject, Resources.EmailTemplate.feedbackBody);

                        body = string.Format(Resources.EmailTemplate.fromBody, model.UserName, model.Email, model.Title, model.Content);
                        this.SendEmailMessage(model.Email, Resources.EmailTemplate.fromSubject, body);

                        this.ModelState.Clear();
                        model = new Contact();
                    }
                }
                return View(model);
            }

            public ActionResult About()
            {
                IAbout _about = new AboutBO();
                vw_About model = _about.GetData(_session.SiteID, _session.LangID);
                return View(model);
            }

            public ActionResult Video()
            {
                IPhoto _cls = new PhotoBO();
                List<vw_Photo> model = new List<vw_Photo>();
                model = _cls.GetData(_session.LangID, false);
                return View(model);
            }

            public ActionResult Gallery()
            {
                IPhoto _cls = new PhotoBO();
                List<vw_Photo> model = new List<vw_Photo>();
                model = _cls.GetData(_session.LangID, true);
                ViewBag.DefaultID = 1;
                return View(model);
            }

            public ActionResult Photo(long id)
            {
                List<vw_PhotoDetail> data = new List<vw_PhotoDetail>();
                IPhoto _cls = new PhotoBO();
                data = _cls.GetPhotoDetail(_session.LangID, id);
                return PartialView(data);
            }

            public ActionResult ChangePassword()
            {
                return View();
            }

            public ActionResult ForgetPassword()
            {
                var model = new NailShop.Business.Model.ModelWeb.ForgetPassword();
                return View(model);
            }

            [HttpPost]
            public ActionResult ForgetPassword(NailShop.Business.Model.ModelWeb.ForgetPassword model)
            {
                string Password = GeneratePassword();
                ICustomer cls = new CustomerBO();
                if (cls.ForgetPassword(model.Email, Password))
                {
                    string body = string.Format(Resources.EmailTemplate.ForgetPasswordBody, model.Email, Password);
                    this.SendEmailMessage(model.Email, Resources.EmailTemplate.ForgetPasswordSubject, body);
                    ViewBag.Message = Resources.ChangePassword.msgSendEmailSucess;
                }
                else
                    ViewBag.Message = Resources.ChangePassword.msgNoteError;
                return View(model);
            }

            private string GeneratePassword()
            {
                string strPwdchar = "abcdefghijklmnopqrstuvwxyz0123456789#+@&$ABCDEFGHIJKLMNOPQRSTUVWXYZ";
                string strPwd = "";
                Random rnd = new Random();
                for (int i = 0; i <= 7; i++)
                {
                    int iRandom = rnd.Next(0, strPwdchar.Length - 1);
                    strPwd += strPwdchar.Substring(iRandom, 1);
                }
                return strPwd;
            }

        #endregion

        #region JsonResult
        
            [HttpPost]
            public JsonResult Login(Business.Model.ModelWeb.LoginModel model)
            {
                ICustomer _customer = new CustomerBO();
                Customer data = _customer.Login(model.LoginName, model.Password);
                if (data != null)
                {
                    _session.IsLogin = true;
                    _session.IsStore = false;
                    _session.CustomerID = data.CustomerID;
                    _session.FullName = data.FullName;
                    _session.StoreID = data.StoreID;
                    return Json(new { IsOk = true }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { IsOk = false }, JsonRequestBehavior.AllowGet);
            }

            [HttpGet]
            public JsonResult GetdataJS()
            {
                ICustomer _cls = new CustomerBO();
                List<Customer> data = _cls.GetList(s => s.StoreID.Equals(_session.IsLogin));
                var select = from c in data
                             select new { c.CustomerID, c.FullName, c.LocalID, c.LoginName };

                string jsonData = new JavaScriptSerializer().Serialize(select.ToList());
                return Json(jsonData, JsonRequestBehavior.AllowGet);
            }

            [HttpPost]
            public JsonResult Logout()
            {
               if(_session.IsLogin)
               {
                   _session.IsLogin = false;
                   _session.StoreID = -1;
                   _session.FullName = null;
                   return Json(new { IsOk = true }, JsonRequestBehavior.AllowGet);
               }
               else
                   return Json(new { IsOk = false }, JsonRequestBehavior.AllowGet);
            }

        #endregion
    }
}