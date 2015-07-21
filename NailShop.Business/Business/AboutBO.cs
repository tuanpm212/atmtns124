using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NailShop.DataAccess;

namespace NailShop.Business
{
    public class AboutBO: RepositoryData<vw_About>, IAbout
    {
        public vw_About GetData(int SiteID, string LangID)
        {
            using (var db = new NailShopEntities())
            {
                var select = from c in db.vw_About
                              where c.SiteID == SiteID && c.LangID == LangID && c.IsActive
                              orderby c.Sort
                              select c;
                if (select.Count() > 0)
                    return select.First();
                return null;
            }
        }

        public bool UpdateAbout(AboutLang aboutLang)
        {
            try
            {
                using (var db = new NailShopEntities())
                {
                    db.Entry(aboutLang).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }
    }
}
