using NailShop.Utils;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;

namespace NailShop.Controllers
{
    public class BaseController : Controller
    {
        #region Public Methods

        protected ISession _session;
        public BaseController()
            : this(new Session())
        {
            if (_session.LangID == null)
                _session.LangID = GetValueByKeyWord(Constant.LANGUAGE).ToString();
            @ViewBag.LangID = _session.LangID;
            if (_session.IsLogin)
            {
                ViewBag.IsLogin = true;
                ViewBag.FullName = _session.FullName;
                ViewBag.StoreID = _session.StoreID;
                ViewBag.IsStore = _session.IsStore;
                ViewBag.NickName = _session.NickName;
                ViewBag.IsAdmin = _session.IsAdmin;
            }
            else
            {
                ViewBag.IsLogin = false;
                ViewBag.FullName = "";
                ViewBag.StoreID = "-1";
                ViewBag.IsStore = false;
                ViewBag.NickName = "";
                ViewBag.IsAdmin = false;
            }
        }

        public BaseController(ISession sessions)
        {
            _session = sessions;
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            InitCulture();
            base.OnActionExecuting(filterContext);
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            try
            {
                filterContext.ExceptionHandled = true;
                ViewBag.DevMessage = filterContext.Exception;
                Response.Redirect("Error", false);
                Response.End();
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }
        }

        public bool SendEmailMessage(string mail, string mailTitle, string mailBody)
        {

            return SendMail(mailBody, "smtp.gmail.com", 587,
                        GetValueByKeyWord(Constant.UserSendMail),
                        GetValueByKeyWord(Constant.PassSendMail),
                        "NailShop.",
                        mail,
                        null,
                        mailTitle,
                        null);
        }

        public static string GetValueByKeyWord(string Keyword)
        {
            return WebConfigurationManager.AppSettings[Keyword];
        }

        #endregion

        #region Private Methods

        private void InitCulture()
        {
            try
            {
                HttpContext context = System.Web.HttpContext.Current;
                if (context.Request.Cookies[Constant.LANGUAGE] != null)
                {
                    _session.LangID = context.Request.Cookies[Constant.LANGUAGE].Value;
                    if (_session.LangID == "undefined")
                    {
                        _session.LangID = GetValueByKeyWord(Constant.LANGUAGE).ToString();
                        HttpCookie cookielangID = new HttpCookie(Constant.LANGUAGE, _session.LangID);
                    }
                }
                else
                {
                    _session.LangID = GetValueByKeyWord(Constant.LANGUAGE).ToString();
                    HttpCookie cookielangID = new HttpCookie(Constant.LANGUAGE, _session.LangID);
                    context.Request.Cookies.Add(cookielangID);
                }

                ViewBag.LangID = _session.LangID;
                var cultureInfo = new CultureInfo(_session.LangID);
                Thread.CurrentThread.CurrentUICulture = cultureInfo;
                Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(cultureInfo.Name);
            }
            catch { }
        }

        private bool SendMail(string mailBody, string mailServer, int mailPost, string mailFrom,
                          string mailFromPass, string mailFromName, string mailTo, string mailCC, string mailSubject,
                          params string[] fileAttachment)
        {
            try
            {
                using (var client = new SmtpClient(mailServer, mailPost))
                {
                    client.EnableSsl = true;
                    client.Credentials = new NetworkCredential(mailFrom, mailFromPass);
                    var mail = new MailMessage();
                    mail.From = new MailAddress(mailFrom);
                    mail.To.Add(mailTo);
                    if (!string.IsNullOrEmpty(mailCC))
                        mail.CC.Add(mailCC);
                    mail.Subject = mailSubject;
                    mail.Body = mailBody;
                    mail.IsBodyHtml = true;
                    client.Send(mail);
                    return true;
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