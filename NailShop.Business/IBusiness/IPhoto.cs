using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NailShop.DataAccess;
using System.Collections;

namespace NailShop.Business
{
    public interface IPhoto : IRepository<vw_Photo>
    {
        List<vw_Photo> GetData(string LangID, bool IsPhoto);
        
        List<vw_PhotoDetail> GetPhotoDetail(string LangID, long PhotoID);
        bool DeletePhoto(long id);

        bool AddNewPhoto(Photo photo, PhotoLang photoLang, List<PhotoDetail> detail, List<PhotoDetailLang> detailLang);

        bool SavePhoto(Photo photo, PhotoLang photoLang, List<PhotoDetail> detail, List<PhotoDetailLang> detailLang);

        bool SaveVideo(Photo photo, PhotoLang photoLang);

        vw_Photo GetVideoByID(string LangID, long id);
        ArrayList GetPhotoByID(string LangID, long id);
    }
}
