using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NailShop.DataAccess;
using System.Transactions;
using System.Collections;

namespace NailShop.Business
{
    public class PhotoBO: RepositoryData<vw_Photo>, IPhoto
    {
        public List<vw_Photo> GetData(string LangID, bool IsPhoto)
        {
            try
            {
                using (var db = new NailShopEntities())
                {
                    var photos = from c in db.vw_Photo
                                      where c.LangID == LangID && c.IsActive && c.IsPhoto == IsPhoto
                                      select c;
                    if (photos.Count() > 0)
                        return photos.ToList();
                    return null;
                }
            }
            catch
            {
                return null;
            }
        }

        public vw_Photo GetVideoByID(string LangID, long id)
        {
            try
            {
                using (var db = new NailShopEntities())
                {
                    var photos = from c in db.vw_Photo
                                 where c.LangID == LangID && c.ID==id
                                 select c;
                    if (photos.Count() > 0)
                        return photos.First();
                    return null;
                }
            }
            catch
            {
                return null;
            }
        }

        public ArrayList GetPhotoByID(string LangID, long id)
        {
            try
            {
                using (var db = new NailShopEntities())
                {
                    ArrayList arrResult = new ArrayList();
                    var photos = from c in db.vw_Photo
                                 where c.LangID == LangID && c.ID == id
                                 select c;
                    if (photos.Count() > 0)
                        arrResult.Add(photos.First());

                    var detail = from c in db.vw_PhotoDetail
                                 where c.LangID == LangID && c.PhotoID == id
                                 select c;
                    if (detail.Count() > 0)
                        arrResult.Add(detail.ToList());
                    return arrResult;
                }
            }
            catch
            {
                return null;
            }
        }

        public List<vw_PhotoDetail> GetPhotoDetail(string LangID, long PhotoID)
        {
            try
            {
                using (var db = new NailShopEntities())
                {
                    var photoDetail = from c in db.vw_PhotoDetail
                                      where c.LangID == LangID && c.PhotoID==PhotoID && c.IsActive
                                      select c;
                    if (photoDetail.Count() > 0)
                       return photoDetail.ToList();
                    return null;
                }
            }
            catch
            {
                return null;
            }
        }

        public bool DeletePhoto(long id)
        {
            try
            {
                using (var db = new NailShopEntities())
                {
                    var row = db.Photos.Find(id);
                    db.Entry(row).State = System.Data.Entity.EntityState.Deleted;
                    db.SaveChanges();
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        public bool AddNewPhoto(Photo photo, PhotoLang photoLang, List<PhotoDetail> detail, List<PhotoDetailLang> detailLang)
        {
            try
            {
                using (var db = new NailShopEntities())
                {
                    using (TransactionScope transcope = new TransactionScope())
                    {
                        try
                        {
                            db.Photos.Add(photo);
                            db.SaveChanges();
                            photoLang.PhotoID = photo.ID;

                            foreach(PhotoDetail row in detail)
                            {
                                row.PhotoID = photo.ID;
                            }
                            db.PhotoDetails.AddRange(detail);
                            db.SaveChanges();

                            for(int i=0; i<  detailLang.Count; i++)
                            {
                                detailLang[i].DetailID = detail[i].DetailID;
                            }
                            db.PhotoDetailLangs.AddRange(detailLang);
                            db.SaveChanges();
                            transcope.Complete();
                            return true;
                        }
                        catch
                        {
                            transcope.Dispose();
                            return false;
                        }
                    }
                }
            }
            catch
            {
                return false;
            }
        }

        public bool SavePhoto(Photo photo, PhotoLang photoLang, List<PhotoDetail> detail, List<PhotoDetailLang> detailLang)
        {
            try
            {
                using (var db = new NailShopEntities())
                {
                    using (TransactionScope transcope = new TransactionScope())
                    {
                        try
                        {
                            if(photo.ID==-1)
                            {
                                db.Photos.Add(photo);
                                db.SaveChanges();
                                photoLang.PhotoID = photo.ID;
                                db.PhotoLangs.Add(photoLang);
                                
                                foreach (PhotoDetail row in detail)
                                {
                                    row.PhotoID = photo.ID;
                                }
                                db.PhotoDetails.AddRange(detail);
                                db.SaveChanges();

                                for (int i = 0; i < detailLang.Count; i++)
                                {
                                    detailLang[i].DetailID = detail[i].DetailID;
                                }
                                db.PhotoDetailLangs.AddRange(detailLang);
                                db.SaveChanges();
                                transcope.Complete();
                                return true;
                            }
                            else
                            {
                                db.Entry(photo).State = System.Data.Entity.EntityState.Modified;
                                db.Entry(photoLang).State = System.Data.Entity.EntityState.Modified;

                                db.PhotoDetails.RemoveRange(db.PhotoDetails.Where(c => c.PhotoID == photo.ID));
                                db.SaveChanges();
                                foreach(PhotoDetail row in detail)
                                {
                                    row.PhotoID = photo.ID;
                                }
                                db.PhotoDetails.AddRange(detail);
                                db.SaveChanges();
                                for(int i=0; i<detailLang.Count; i++)
                                {
                                    detailLang[i].DetailID = detail[i].DetailID;
                                }
                                db.PhotoDetailLangs.AddRange(detailLang);
                                db.SaveChanges();
                                transcope.Complete();
                                return true;
                            }
                        }
                        catch
                        {
                            transcope.Dispose();
                            return false;
                        }
                    }
                }
            }
            catch
            {
                return false;
            }
        }

        public bool SaveVideo(Photo photo, PhotoLang photoLang)
        {
            try
            {
                using (var db = new NailShopEntities())
                {
                    using (TransactionScope transcope = new TransactionScope())
                    {
                        try
                        {
                            if(photo.ID==-1)
                            {
                                db.Photos.Add(photo);
                                db.SaveChanges();
                                photoLang.PhotoID = photo.ID;
                                db.PhotoLangs.Add(photoLang);
                                db.SaveChanges();
                                transcope.Complete();
                                return true;
                            }
                            else
                            {
                                db.Entry(photo).State = System.Data.Entity.EntityState.Modified;
                                db.Entry(photoLang).State = System.Data.Entity.EntityState.Modified;
                                db.SaveChanges();
                                transcope.Complete();
                                return true;
                            }
                        }
                        catch
                        {
                            transcope.Dispose();
                            return false;
                        }
                    }
                }
            }
            catch
            {
                return false;
            }
        }

    }
}
