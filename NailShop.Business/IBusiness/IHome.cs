using NailShop.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NailShop.Business
{
    public interface IHome: IRepository<vw_Slide>
    {
        Model.ModelWeb.Home GetHomeData(int SiteID, string LangID);

    }
}
