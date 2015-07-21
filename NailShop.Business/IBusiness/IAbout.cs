using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NailShop.DataAccess;

namespace NailShop.Business
{
    public interface IAbout: IRepository<vw_About>
    {
        vw_About GetData(int SiteID, string LangID);
        bool UpdateAbout(AboutLang aboutLang);
    }
}
