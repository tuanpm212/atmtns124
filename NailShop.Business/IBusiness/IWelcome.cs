using NailShop.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NailShop.Business
{
    public interface IWelcome : IRepository<vw_Slide>
    {
        vw_Slide GetWelcome(string LangID);

        bool SaveWelcome(Slide slide, SlideLang slideLang);
    }
}
