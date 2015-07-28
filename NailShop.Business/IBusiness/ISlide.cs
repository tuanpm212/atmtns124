using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NailShop.DataAccess;

namespace NailShop.Business
{
    public interface ISlide : IRepository<vw_Slide>
    {
        vw_Slide GetData(long ID);

        bool Delete(long ID);

        List<vw_Slide> GetSlide(string LangID, string Type);

        bool SaveSlide(Slide slide, SlideLang slideLang);
    }
}
