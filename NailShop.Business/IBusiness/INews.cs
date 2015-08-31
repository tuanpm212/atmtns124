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
        bool Save(News news, NewsLang newsLang);
        bool Delete(long ID);
        vw_News GetData(string LangID, long ID);
        List<vw_News> GetData(string LangID);
    }
}
