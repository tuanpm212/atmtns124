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
        bool AddNew(Slide slide, SlideLang slideLang);
        bool Edit(Slide slide, SlideLang slideLang);
    }
}
