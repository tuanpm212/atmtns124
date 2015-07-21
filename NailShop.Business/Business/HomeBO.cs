using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NailShop.DataAccess;

namespace NailShop.Business
{
    public class HomeBO: RepositoryData<vw_Slide>, IHome
    {
        public Model.ModelWeb.Home GetHomeData(int SiteID, string LangID)
        {
            try
            {
                Model.ModelWeb.Home model = new Model.ModelWeb.Home();
                using (var db = new NailShopEntities())
                {
                    var _slider = from c in db.vw_Slide
                                  where c.SiteID == SiteID && c.LangID == LangID && c.IsActive && c.Type == "SLIDE_TOP"
                                  orderby c.Sort
                                  select c;
                    if (_slider.Count() > 0)
                        model.Sliders = _slider.ToList();

                    var _welcome = from c in db.vw_Slide
                                   where c.SiteID == SiteID && c.LangID == LangID && c.IsActive && c.Type == "WELCOME"
                                   orderby c.Sort
                                   select c;
                    if (_welcome.Count() > 0)
                        model.Welcome = _welcome.First();

                    var _news = from c in db.vw_News
                                   where c.SiteID == SiteID && c.LangID == LangID && c.IsActive
                                   orderby c.Sort
                                   select c;
                    if (_news.Count() > 0)
                        model.News = _news.ToList();

                    var _brand = from c in db.vw_Brand
                                where c.SiteID == SiteID && c.LangID == LangID && c.IsActive
                                orderby c.Sort
                                select c;
                    if (_brand.Count() > 0)
                        model.Brands = _brand.ToList();

                    var _store = from c in db.Stores
                                 where c.FromDate <= DateTime.Now && c.ToDate>= DateTime.Now &&  c.IsActvie
                                 orderby c.Sort
                                 select c;
                    if (_store.Count() > 0)
                        model.Stores = _store.ToList();

                    var _product = from c in db.vw_ProductHot
                                   where c.SiteID == SiteID && c.IsActive
                                   orderby c.StoreID, c.Sort
                                   select c;
                    if (_product.Count() > 0)
                        model.ProductHots = _product.ToList();
                    return model;
                }
            }
            catch
            {
                return null;
            }
        }
    }
}
