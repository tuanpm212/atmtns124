using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NailShop.DataAccess;

namespace NailShop.Business
{
    public interface INews : IRepository<vw_News>
    {
        bool AddNew(News news, NewsLang newsLang);
        bool Edit(News news, NewsLang newsLang);
    }
}
