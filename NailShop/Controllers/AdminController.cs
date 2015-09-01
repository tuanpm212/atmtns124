using NailShop.Business;
using NailShop.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Web.Script.Serialization;

namespace NailShop.Controllers
{
    public class AdminController : BaseController
    {
        #region Action Result
            public ActionResult Index()
            {
                return View();
            }

        #endregion

        #region Photo
            public ActionResult Photo()
            {
                if (_session.IsLogin && _session.IsStore && _session.IsAdmin)
                    return View();
                else
                    return RedirectToAction("index", "admin");
            }

            public ActionResult CreatePhoto(long id)
            {
                if (_session.IsLogin && _session.IsStore && _session.IsAdmin)
                {
                    ViewBag.ID = id;
                    return View();
                }
                else
                    return RedirectToAction("index", "admin");
            }
        #endregion

        #region Video
            public ActionResult Video()
            {
                if (_session.IsLogin && _session.IsStore && _session.IsAdmin)
                    return View();
                else
                    return RedirectToAction("index", "admin");
            }

            public ActionResult CreateVideo(long id)
            {
                ViewBag.ID = id;
                if (_session.IsLogin && _session.IsStore && _session.IsAdmin)
                {
                    IPhoto cls = new PhotoBO();
                    var data = cls.GetVideoByID(_session.LangID, id);
                    if (data != null)
                    {
                        ViewBag.Name = data.Title;
                        ViewBag.Description = data.Description;
                        ViewBag.Video = data.Video;
                        ViewBag.IsActive = data.IsActive;
                    }
                    else
                    {
                        ViewBag.Name = "";
                        ViewBag.Description = "";
                        ViewBag.Video = "";
                        ViewBag.IsActive = true;
                    }
                    return View();
                }
                else
                    return RedirectToAction("index", "admin");
            }

            [HttpPost]
            public JsonResult SaveVideo(Photo photo, PhotoLang photolang)
            {
                IPhoto cls = new PhotoBO();
                bool IsResult = cls.SaveVideo(photo, photolang);
                return Json(new { IsOk = IsResult }, JsonRequestBehavior.AllowGet);
            }

        #endregion

        #region Abouts
            public ActionResult About()
            {
                if (_session.IsLogin && _session.IsStore && _session.IsAdmin)
                {
                    IAbout _cls = new AboutBO();
                    var model = _cls.GetData(_session.SiteID, _session.LangID);
                    return View(model);
                }
                else
                    return RedirectToAction("index", "admin");
            }

            [HttpPost]
            public JsonResult About(AboutLang aboutlang)
            {
                if (_session.IsLogin && _session.IsStore && _session.IsAdmin)
                {
                    IAbout _cls = new AboutBO();
                    var isResult = _cls.UpdateAbout(aboutlang);
                    return Json(new { IsOk = isResult }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    RedirectToAction("index", "admin");
                    return Json(new { IsOk = false }, JsonRequestBehavior.AllowGet);
                }
            }

        #endregion

        #region Slider
            public ActionResult Slide()
            {
                if (_session.IsLogin && _session.IsStore && _session.IsAdmin)
                    return View();
                else
                    return RedirectToAction("index", "admin");
            }

            public ActionResult CreateSlide(long id)
            {
                if (_session.IsLogin && _session.IsStore && _session.IsAdmin)
                {
                    ViewBag.ID = id;
                    ViewBag.Image = "~/Uploads/Default/default.png";
                    vw_Slide model = new vw_Slide();
                    if (id != -1)
                    {
                        ISlide cls = new SlideBO();
                        model = cls.GetData(id);
                        if (model == null)
                            model = new vw_Slide();
                        else
                            ViewBag.Image = model.Image;
                    }
                    return View(model);
                }
                else
                    return RedirectToAction("index", "admin");
            }
        #endregion

        #region Welcome
            public ActionResult Welcome()
            {
                if (_session.IsLogin && _session.IsStore && _session.IsAdmin)
                {
                    IWelcome _cls = new WelcomeBO();
                    var model = _cls.GetWelcome(_session.LangID);
                    return View(model);
                }
                else
                    return RedirectToAction("index", "admin");
            }

        #endregion

        #region Brand
            public ActionResult Brand()
            {
                if (_session.IsLogin && _session.IsStore && _session.IsAdmin)
                    return View();
                else
                    return RedirectToAction("index", "admin");
            }

            public ActionResult CreateBrand(long id)
            {
                if (_session.IsLogin && _session.IsStore && _session.IsAdmin)
                {
                    vw_Brand model = new vw_Brand();
                    ViewBag.ID = id;
                    if(id!=-1)
                    {
                        IBrand cls = new BrandBO();
                        model = cls.GetData(_session.LangID, id);
                    }
                    return View(model);
                }
                else
                    return RedirectToAction("index", "admin");
            }
        #endregion

        #region News
            public ActionResult News()
            {
                if (_session.IsLogin && _session.IsStore && _session.IsAdmin)
                    return View();
                else
                    return RedirectToAction("index", "admin");
            }

            public ActionResult CreateNews(long id)
            {
                if (_session.IsLogin && _session.IsStore && _session.IsAdmin)
                {
                    vw_News model = new vw_News();
                    ViewBag.ID = id;
                    if (id != -1)
                    {
                        INews cls = new NewsBO();
                        model = cls.GetData(_session.LangID, id);
                    }
                    return View(model);
                }
                else
                    return RedirectToAction("index", "admin");
            }
        #endregion

        #region Product Hot
            public ActionResult ProductHot()
            {
                if (_session.IsLogin && _session.IsStore && _session.IsAdmin)
                    return View();
                else
                    return RedirectToAction("index", "admin");
            }

            public ActionResult CreateProductHot(int store, long id)
            {
                if (_session.IsLogin && _session.IsStore && _session.IsAdmin)
                {
                    vw_ProductHot model = new vw_ProductHot();
                    ViewBag.ID = id;
                    @ViewBag.StoreID = store;
                    if (id != -1)
                    {
                        IProductHot cls = new ProductHotBO();
                        model = cls.GetData(id);
                    }
                    return View(model);
                }
                else
                    return RedirectToAction("index", "admin");
            }
            #endregion

        #region JsonResult

            [HttpPost]
            public JsonResult Login(Business.Model.ModelWeb.LoginModel model)
            {
                IStore _cls = new StoreBO();
                Store _store = _cls.Login(model.LoginName, model.Password);
                if (_store != null && _store.IsAdmin)
                {
                    _session.IsLogin = true;
                    _session.IsStore = true;
                    _session.CustomerID = -1;
                    _session.FullName = _store.StoreName;
                    _session.StoreID = _store.StoreID;
                    _session.IsAdmin = _store.IsAdmin;
                    return Json(new { IsOk = true }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    _session.IsLogin = false;
                    _session.IsStore = false;
                    _session.StoreID = -1;
                    _session.IsAdmin = false;
                    return Json(new { IsOk = false }, JsonRequestBehavior.AllowGet);
                }
            }

            [HttpPost]
            public JsonResult UploadCover()
            {
                // Validate we have a file being posted
                if (Request.Files.Count == 0)
                {
                    return Json(new { statusCode = 500, status = "No image provided." }, "text/html");
                }
                // File we want to resize and save.
                var file = Request.Files[0];

                try
                {
                    var filename = UploadCover(file);
                    return Json(new
                    {
                        statusCode = 200,
                        status = "Image uploaded.",
                        file = filename,
                    }, "text/html");
                }
                catch (Exception ex)
                {
                    return Json(new
                    {
                        statusCode = 500,
                        status = "Error uploading image." + ex.Message,
                        file = string.Empty
                    }, "text/html");
                }
            }

            [HttpPost]
            public JsonResult UploadPhoto()
            {
                // Validate we have a file being posted
                if (Request.Files.Count == 0)
                {
                    return Json(new { statusCode = 500, status = "No image provided." }, "text/html");
                }
                // File we want to resize and save.
                var file = Request.Files[0];

                try
                {
                    var filename = UploadPhoto(file);
                    return Json(new
                    {
                        statusCode = 200,
                        status = "Image uploaded.",
                        file = filename,
                        id = Guid.NewGuid().ToString(),
                    }, "text/html");
                }
                catch (Exception ex)
                {
                    return Json(new
                    {
                        statusCode = 500,
                        status = "Error uploading image." + ex.Message,
                        file = string.Empty
                    }, "text/html");
                }
            }

            [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
            public JsonResult GetPhoto()
            {
                string jsonData = "[]";
                IPhoto _cls = new PhotoBO();
                var data = _cls.GetData(_session.LangID, true);
                if (data != null)
                    jsonData = new JavaScriptSerializer().Serialize(data);
                return Json(jsonData, JsonRequestBehavior.AllowGet);
            }

            public JsonResult GetPhotoByID(long id)
            {
                if (_session.IsLogin && _session.IsStore && _session.IsAdmin)
                {
                    if (id == -1)
                    {
                        string jsonData = "";
                        jsonData = "{\"master\":" + new JavaScriptSerializer().Serialize(new vw_Photo());
                        jsonData += ",\"detail\":[]}";
                        return Json(jsonData, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        string jsonData = "";
                        IPhoto _cls = new PhotoBO();
                        var data = _cls.GetPhotoByID(_session.LangID, id);
                        jsonData = "{\"master\":" + new JavaScriptSerializer().Serialize(data[0]);
                        jsonData += ",\"detail\":" + new JavaScriptSerializer().Serialize(data[1]) + "}"; ;
                        return Json(jsonData, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                    RedirectToAction("index", "admin");
                return Json("[]", JsonRequestBehavior.AllowGet);
            }

            [HttpPost]
            public JsonResult DeletePhoto(long id)
            {
                if (_session.IsLogin && _session.IsStore && _session.IsAdmin)
                {
                    string jsonData = "[]";
                    IPhoto _cls = new PhotoBO();
                    var data = _cls.DeletePhoto(id);
                    jsonData = new JavaScriptSerializer().Serialize(data);
                    return Json(jsonData, JsonRequestBehavior.AllowGet);
                }
                else
                    RedirectToAction("index", "admin");
                return Json("[]", JsonRequestBehavior.AllowGet);
            }

            [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
            public JsonResult GetVideo()
            {
                if (_session.IsLogin && _session.IsStore && _session.IsAdmin)
                {
                    string jsonData = "[]";
                    IPhoto _cls = new PhotoBO();
                    var data = _cls.GetData(_session.LangID, false);
                    if (data != null)
                        jsonData = new JavaScriptSerializer().Serialize(data);
                    return Json(jsonData, JsonRequestBehavior.AllowGet);
                }
                else
                    RedirectToAction("index", "backend");
                return Json("[]", JsonRequestBehavior.AllowGet);
            }

            [HttpPost]
            public JsonResult AddNewPhoto(Photo photo, PhotoLang photoLang, List<PhotoDetail> detail, List<PhotoDetailLang> detailLang)
            {
                if (_session.IsLogin && _session.IsStore && _session.IsAdmin)
                {
                    IPhoto _cls = new PhotoBO();
                    var IsResult = _cls.AddNewPhoto(photo, photoLang, detail, detailLang);
                    return Json(new { IsOk = IsResult }, JsonRequestBehavior.AllowGet);
                }
                else
                    RedirectToAction("index", "backend");
                return Json("[]", JsonRequestBehavior.AllowGet);
            }

            [HttpPost]
            public JsonResult SavePhoto(Photo photo, PhotoLang photoLang, List<PhotoDetail> detail, List<PhotoDetailLang> detailLang)
            {
                if (_session.IsLogin && _session.IsStore && _session.IsAdmin)
                {
                    IPhoto _cls = new PhotoBO();
                    var IsResult = _cls.SavePhoto(photo, photoLang, detail, detailLang);
                    return Json(new { IsOk = IsResult }, JsonRequestBehavior.AllowGet);
                }
                else
                    RedirectToAction("index", "backend");
                return Json("[]", JsonRequestBehavior.AllowGet);
            }

            private string UploadCover(HttpPostedFileBase file)
            {
                // Initialize variables we'll need for resizing and saving
                var width = 240;
                var height = 173;
                var relativeFileAndPath = "/Uploads/default";
                var fileName = Guid.NewGuid().ToString() + file.FileName.Substring(file.FileName.LastIndexOf('.'));
                // Build absolute path
                var absPath = HttpContext.Server.MapPath(relativeFileAndPath);
                var absFileAndPath = absPath + "\\" + fileName;

                // Create directory as necessary and save image on server
                if (!Directory.Exists(absPath))
                    Directory.CreateDirectory(absPath);
                file.SaveAs(absFileAndPath);

                // Resize image using "ImageResizer" NuGet package.
                var resizeSettings = new ImageResizer.ResizeSettings
                {
                    Scale = ImageResizer.ScaleMode.DownscaleOnly,
                    Width = width,
                    Height = height,
                    Mode = ImageResizer.FitMode.Crop
                };
                var b = ImageResizer.ImageBuilder.Current.Build(absFileAndPath, resizeSettings);

                // Save resized image
                b.Save(absFileAndPath);

                // Return relative file path
                return relativeFileAndPath + "/" + fileName;
            }

            private string UploadPhoto(HttpPostedFileBase file)
            {
                // Initialize variables we'll need for resizing and saving
                var widthThumnail = 240;
                var heightThumnail = 173;
                var widthLightBox = 1074;
                var heightLightBox = 768;
                var relativeFileAndPath = "/Uploads/default";
                var idFile = Guid.NewGuid().ToString();
                var fileNameThumnail = idFile + "_thumnail" + file.FileName.Substring(file.FileName.LastIndexOf('.'));
                var fileNameLightBox = idFile + "_lightbox" + file.FileName.Substring(file.FileName.LastIndexOf('.'));
                var fileNameFull = idFile + file.FileName.Substring(file.FileName.LastIndexOf('.'));
                // Build absolute path
                var absPath = HttpContext.Server.MapPath(relativeFileAndPath);
                var absFileThumnail = absPath + "\\" + fileNameThumnail;
                var absFileLightBox = absPath + "\\" + fileNameLightBox;
                var absFileFull = absPath + "\\" + fileNameFull;

                // Create directory as necessary and save image on server
                if (!Directory.Exists(absPath))
                    Directory.CreateDirectory(absPath);
                file.SaveAs(absFileThumnail);
                file.SaveAs(absFileLightBox);
                file.SaveAs(absFileFull);

                // Resize image using "ImageResizer" NuGet package.
                var resizeThumnail = new ImageResizer.ResizeSettings
                {
                    Scale = ImageResizer.ScaleMode.DownscaleOnly,
                    Width = widthThumnail,
                    Height = heightThumnail,
                    Mode = ImageResizer.FitMode.Crop
                };
                var b = ImageResizer.ImageBuilder.Current.Build(absFileThumnail, resizeThumnail);
                var resizeLightBox = new ImageResizer.ResizeSettings
                {
                    Scale = ImageResizer.ScaleMode.DownscaleOnly,
                    Width = widthLightBox,
                    Height = heightLightBox,
                    Mode = ImageResizer.FitMode.Crop
                };
                var c = ImageResizer.ImageBuilder.Current.Build(absFileThumnail, resizeLightBox);
                // Save resized image
                b.Save(absFileThumnail);
                c.Save(absFileLightBox);

                // Return relative file path
                return relativeFileAndPath + "/" + fileNameFull;
            }

            [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
            public JsonResult GetSlider()
            {
                string jsonData = "[]";
                ISlide _cls = new SlideBO();
                var data = _cls.GetSlide(_session.LangID, "SLIDE_TOP");
                if (data != null)
                    jsonData = new JavaScriptSerializer().Serialize(data);
                return Json(jsonData, JsonRequestBehavior.AllowGet);
            }

            [HttpPost]
            public JsonResult DeleteSlide(long id)
            {
                if (_session.IsLogin && _session.IsStore && _session.IsAdmin)
                {
                    ISlide _cls = new SlideBO();
                    var IsResult = _cls.Delete(id);
                    return Json(new { IsOk = IsResult}, JsonRequestBehavior.AllowGet);
                }
                else
                    RedirectToAction("index", "admin");
                return Json(new { IsOk = false}, JsonRequestBehavior.AllowGet);
            }

            [HttpPost]
            public JsonResult SaveSlide(Slide slide, SlideLang slideLang)
            {
                if (_session.IsLogin && _session.IsStore && _session.IsAdmin)
                {
                    slide.SiteID = _session.SiteID;
                    slideLang.LangID = _session.LangID;
                    ISlide _cls = new SlideBO();
                    var IsResult = _cls.SaveSlide(slide, slideLang);
                    return Json(new { IsOk = IsResult }, JsonRequestBehavior.AllowGet);
                }
                else
                    RedirectToAction("index", "admin");
                return Json("[]", JsonRequestBehavior.AllowGet);
            }

            public JsonResult SaveWelcome(Slide slide, SlideLang slideLang)
            {
                if (_session.IsLogin && _session.IsStore && _session.IsAdmin)
                {
                    slide.SiteID = _session.SiteID;
                    slide.Type = "WELCOME";
                    slide.IsActive = true;
                    slideLang.LangID = _session.LangID;
                    slideLang.Name = "Welcome";

                    IWelcome _cls = new WelcomeBO();
                    var IsResult = _cls.SaveWelcome(slide, slideLang);
                    return Json(new { IsOk = IsResult }, JsonRequestBehavior.AllowGet);
                }
                else
                    RedirectToAction("index", "admin");
                return Json("[]", JsonRequestBehavior.AllowGet);
            }

            [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
            public JsonResult GetBrand()
            {
                string jsonData = "[]";
                IBrand _cls = new BrandBO();
                var data = _cls.GetData(_session.LangID);
                if (data != null)
                    jsonData = new JavaScriptSerializer().Serialize(data);
                return Json(jsonData, JsonRequestBehavior.AllowGet);
            }

            [HttpPost]
            public JsonResult SaveBrand(Brand brand, BrandLang brandLang)
            {
                if (_session.IsLogin && _session.IsStore && _session.IsAdmin)
                {
                    brand.SiteID = _session.SiteID;
                    brandLang.LangID = _session.LangID;
                    IBrand _cls = new BrandBO();
                    var IsResult = _cls.Save(brand, brandLang);
                    return Json(new { IsOk = IsResult }, JsonRequestBehavior.AllowGet);
                }
                else
                    RedirectToAction("index", "admin");
                return Json("[]", JsonRequestBehavior.AllowGet);
            }

            [HttpPost]
            public JsonResult DeleteBrand(long id)
            {
                if (_session.IsLogin && _session.IsStore && _session.IsAdmin)
                {
                    IBrand _cls = new BrandBO();
                    var IsResult = _cls.Delete(id);
                    return Json(new { IsOk = IsResult }, JsonRequestBehavior.AllowGet);
                }
                else
                    RedirectToAction("index", "admin");
                return Json(new { IsOk = false }, JsonRequestBehavior.AllowGet);
            }

            [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
            public JsonResult GetProductHot(int StoreID)
            {
                string jsonData = "[]";
                IProductHot _cls = new ProductHotBO();
                var data = _cls.GetData(StoreID);
                if (data != null)
                    jsonData = new JavaScriptSerializer().Serialize(data);
                return Json(jsonData, JsonRequestBehavior.AllowGet);
            }

            [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
            public JsonResult GetStore()
            {
                string jsonData = "[]";
                IStore _cls = new StoreBO();
                var data = _cls.GetStores();
                if (data == null)
                    data = new List<Store>();
                jsonData = new JavaScriptSerializer().Serialize(data);
                return Json(jsonData, JsonRequestBehavior.AllowGet);
            }

            [HttpPost]
            public JsonResult DeleteProductHot(long id)
            {
                if (_session.IsLogin && _session.IsStore && _session.IsAdmin)
                {
                    IProductHot _cls = new ProductHotBO();
                    var IsResult = _cls.Delete(id);
                    return Json(new { IsOk = IsResult }, JsonRequestBehavior.AllowGet);
                }
                else
                    RedirectToAction("index", "admin");
                return Json(new { IsOk = false }, JsonRequestBehavior.AllowGet);
            }

            [HttpPost]
            public JsonResult SaveProductHot(ProductHot product)
            {
                if (_session.IsLogin && _session.IsStore && _session.IsAdmin)
                {
                    product.SiteID = _session.SiteID;
                    IProductHot _cls = new ProductHotBO();
                    var IsResult = _cls.Save(product);
                    return Json(new { IsOk = IsResult }, JsonRequestBehavior.AllowGet);
                }
                else
                    RedirectToAction("index", "admin");
                return Json("[]", JsonRequestBehavior.AllowGet);
            }

            [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
            public JsonResult GetNews()
            {
                string jsonData = "[]";
                INews _cls = new NewsBO();
                var data = _cls.GetData(_session.LangID);
                if (data != null)
                    jsonData = new JavaScriptSerializer().Serialize(data);
                return Json(jsonData, JsonRequestBehavior.AllowGet);
            }

            [HttpPost]
            public JsonResult SaveNews(News news, NewsLang newsLang)
            {
                if (_session.IsLogin && _session.IsStore && _session.IsAdmin)
                {
                    news.SiteID = _session.SiteID;
                    newsLang.LangID = _session.LangID;
                    INews _cls = new NewsBO();
                    var IsResult = _cls.Save(news, newsLang);
                    return Json(new { IsOk = IsResult }, JsonRequestBehavior.AllowGet);
                }
                else
                    RedirectToAction("index", "admin");
                return Json("[]", JsonRequestBehavior.AllowGet);
            }

            [HttpPost]
            public JsonResult DeleteNews(long id)
            {
                if (_session.IsLogin && _session.IsStore && _session.IsAdmin)
                {
                    INews _cls = new NewsBO();
                    var IsResult = _cls.Delete(id);
                    return Json(new { IsOk = IsResult }, JsonRequestBehavior.AllowGet);
                }
                else
                    RedirectToAction("index", "admin");
                return Json(new { IsOk = false }, JsonRequestBehavior.AllowGet);
            }

        #endregion
    }
}