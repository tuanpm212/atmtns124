using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NailShop.DataAccess;

namespace NailShop.Utils
{
    public interface ISession
    {
        void Clear();
        string LangID { get; set; }
        int SiteID { get; set; }

        bool IsLogin { get; set; }

        bool IsStore { get; set; }

        bool IsAdmin { get; set; }

        int StoreID { get; set; }

        long CustomerID { get; set; }

        string FullName { get; set; }

        string NickName { get; set; }
    }
}