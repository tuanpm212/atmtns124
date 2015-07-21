using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using NailShop.DataAccess;

namespace NailShop.Utils
{
    public class Session:ISession
    {
        public void Clear()
        {
            HttpContext.Current.Session.Clear();
        }

        public int SiteID
        {
            get
            {
                if (HttpContext.Current.Session == null || HttpContext.Current.Session[Constant.SITEID] == null)
                    return Convert.ToInt32(GetValueByKeyWord(Constant.SITEID));
                else
                    return Convert.ToInt32(HttpContext.Current.Session[Constant.SITEID].ToString());
            }
            set { HttpContext.Current.Session[Constant.SITEID] = value; }
        }

        public string LangID
        {
            get
            {
                if (HttpContext.Current.Session == null || HttpContext.Current.Session[Constant.LANGUAGE] == null)
                    return GetValueByKeyWord(Constant.LANGUAGE);
                else
                    return HttpContext.Current.Session[Constant.LANGUAGE].ToString();
            }
            set { HttpContext.Current.Session[Constant.LANGUAGE] = value; }
        }

        public bool IsLogin
        {
            get
            {
                if (HttpContext.Current.Session == null || HttpContext.Current.Session[Constant.IS_LOGIN] == null)
                    return Convert.ToBoolean(GetValueByKeyWord(Constant.IS_LOGIN));
                else
                    return Convert.ToBoolean(HttpContext.Current.Session[Constant.IS_LOGIN].ToString());
            }
            set { HttpContext.Current.Session[Constant.IS_LOGIN] = value; }
        }

        public bool IsAdmin
        {
            get
            {
                if (HttpContext.Current.Session == null || HttpContext.Current.Session[Constant.IS_ADMIN] == null)
                    return Convert.ToBoolean(GetValueByKeyWord(Constant.IS_ADMIN));
                else
                    return Convert.ToBoolean(HttpContext.Current.Session[Constant.IS_ADMIN].ToString());
            }
            set { HttpContext.Current.Session[Constant.IS_ADMIN] = value; }
        }


        public bool IsStore
        {
            get
            {
                if (HttpContext.Current.Session == null || HttpContext.Current.Session[Constant.IS_STORE] == null)
                    return Convert.ToBoolean(GetValueByKeyWord(Constant.IS_STORE));
                else
                    return Convert.ToBoolean(HttpContext.Current.Session[Constant.IS_STORE].ToString());
            }
            set { HttpContext.Current.Session[Constant.IS_STORE] = value; }
        }

        public int StoreID
        {
            get
            {
                if (HttpContext.Current.Session == null || HttpContext.Current.Session[Constant.STOREID] == null)
                    return -1;
                else
                    return Convert.ToInt32(HttpContext.Current.Session[Constant.STOREID].ToString());
            }
            set { HttpContext.Current.Session[Constant.STOREID] = value; }
        }

        public long CustomerID
        {
            get
            {
                if (HttpContext.Current.Session == null || HttpContext.Current.Session[Constant.CUSTOMER_ID] == null)
                    return -1;
                else
                    return Convert.ToInt32(HttpContext.Current.Session[Constant.CUSTOMER_ID].ToString());
            }
            set { HttpContext.Current.Session[Constant.CUSTOMER_ID] = value; }
        }

        public string FullName
        {
            get
            {
                if (HttpContext.Current.Session == null || HttpContext.Current.Session[Constant.FULL_NAME] == null)
                    return null;
                else
                    return HttpContext.Current.Session[Constant.FULL_NAME].ToString();
            }
            set { HttpContext.Current.Session[Constant.FULL_NAME] = value; }
        }

        public string NickName
        {
            get
            {
                if (HttpContext.Current.Session == null || HttpContext.Current.Session[Constant.FULL_NICKNAME] == null)
                    return null;
                else
                    return HttpContext.Current.Session[Constant.FULL_NICKNAME].ToString();
            }
            set { HttpContext.Current.Session[Constant.FULL_NICKNAME] = value; }
        }


        private string GetValueByKeyWord(string Keyword)
        {
            return WebConfigurationManager.AppSettings[Keyword];
        }
    }
}