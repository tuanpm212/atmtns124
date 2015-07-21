using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;

using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace NailShop.Controllers
{
    public class MediaManagerController : Controller
    {
        private string SiteID = "";

        private string contentFolderRoot = "~/Uploads/Editor/";

        public ActionResult Index(string path)
        {
            string root = Server.MapPath(ContentPath);
            root = (String.IsNullOrEmpty(path)) ? root : root + @"\" + path;
            List<object> list = new List<object>();
           
            if (!Directory.Exists(root))
            {
                Directory.CreateDirectory(root);
            }
            string[] files = Directory.GetFiles(root);
            string[] directorys = Directory.GetDirectories(root);
            foreach (string directory in directorys)
            {
                var data = new { name = new DirectoryInfo(directory).Name, type = "d", size = 0 };
                list.Add(data);
            }

            foreach (string file in files)
            {
                var data = new { name = Path.GetFileName(file), type = "f", size = new FileInfo(file).Length };
                list.Add(data);
            }

            return Json(list, JsonRequestBehavior.AllowGet);
        }

        [OutputCache(Duration = 0xe10, VaryByParam = "path")]
        public virtual ActionResult Thumbnail(string path)
        {

            path = this.NormalizePath(path);

            string str = base.Server.MapPath(path);

            if (!System.IO.File.Exists(str))
            {
                throw new HttpException(0x194, "File Not Found");
            }
            base.Response.AddFileDependency(str);
            return this.CreateThumbnail(str);
        }

        public virtual FilePathResult GetImage(string path)
        {
            string root = Server.MapPath(ContentPath);
            root = (String.IsNullOrEmpty(path)) ? root : root + @"\" + path;
            var type = Path.GetExtension(root);
            type = type.Split('.')[1];
            Dictionary<string, string> ImageFormats = new Dictionary<string, string>();
            ImageFormats.Add("png", "image/png");
            ImageFormats.Add("gif", "image/gif");
            ImageFormats.Add("jpeg", "image/jpeg");
            ImageFormats.Add("jpg", "image/jpeg");
            string format = ImageFormats[type.ToLower()];
            return base.File(root, format);
        }

        private string NormalizePath(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                return VirtualPathUtility.ToAbsolute(this.ContentPath);
            }
            return VirtualPathUtility.Combine(VirtualPathUtility.ToAbsolute(this.ContentPath), path);
        }

        private string CreateUserFolder()
        {
            if (!String.IsNullOrEmpty(Request["userfolder"]))
            {
                return Path.Combine(contentFolderRoot, this.SiteID.ToString() + "/"); ;
            }
            else return Path.Combine(contentFolderRoot, "");
        }

        public string ContentPath
        {
            get
            {
                if (!String.IsNullOrEmpty(Request["userfolder"]))
                {
                    SiteID = Request["userfolder"];
                }
                else SiteID = "";

                string folder = CreateUserFolder();
                string root = Server.MapPath(folder);
                if (!Directory.Exists(root))
                {
                    Directory.CreateDirectory(root);
                }

                return CreateUserFolder();
            }
        }

        private FileContentResult CreateThumbnail(string physicalPath)
        {
            using (FileStream stream = System.IO.File.OpenRead(physicalPath))
            {
                ImageSize desiredSize = new ImageSize
                {
                    Width = 80,
                    Height = 80
                };

                var type = Path.GetExtension(physicalPath);
                type = type.Split('.')[1];

                return base.File(Create(stream, desiredSize, "image/" + type.ToLower()), "image/" + type.ToLower());
            }
        }

        public FileContentResult getThumbnail(string path,int width,int height)
        {
            string physicalPath = Server.MapPath(path);
            using (FileStream stream = System.IO.File.OpenRead(physicalPath))
            {
                ImageSize desiredSize = new ImageSize
                {
                    Width = width,
                    Height = height
                };

                var type = Path.GetExtension(physicalPath);
                type = type.Split('.')[1];

                return base.File(Create(stream, desiredSize, "image/" + type.ToLower()), "image/" + type.ToLower());
            }
        }

        public byte[] Create(Stream source, ImageSize desiredSize, string contentType)
        {
            Dictionary<string, ImageFormat> ImageFormats = new Dictionary<string, ImageFormat>();
            ImageFormats.Add("image/png", ImageFormat.Png);
            ImageFormats.Add("image/gif", ImageFormat.Gif);
            ImageFormats.Add("image/jpeg", ImageFormat.Jpeg);
            ImageFormats.Add("image/jpg", ImageFormat.Jpeg);

            using (Image image = Image.FromStream(source))
            {
                ImageSize originalSize = new ImageSize
                {
                    Height = image.Height,
                    Width = image.Width
                };
                ImageSize size3 = Resize(originalSize, desiredSize);
                using (Bitmap bitmap = new Bitmap(size3.Width, size3.Height))
                {
                    this.ScaleImage(image, bitmap);
                    using (MemoryStream stream = new MemoryStream())
                    {
                        bitmap.Save(stream, ImageFormats[contentType]);
                        return stream.ToArray();
                    }
                }
            }
        }

        public ImageSize Resize(ImageSize originalSize, ImageSize targetSize)
        {
            float num = ((float)originalSize.Width) / ((float)originalSize.Height);
            int width = targetSize.Width;
            int height = targetSize.Height;
            if ((originalSize.Width > targetSize.Width) || (originalSize.Height > targetSize.Height))
            {
                if (num > 1f)
                {
                    height = (int)(((float)targetSize.Height) / num);
                }
                else
                {
                    width = (int)(targetSize.Width * num);
                }
            }
            else
            {
                width = originalSize.Width;
                height = originalSize.Height;
            }
            return new ImageSize { Width = Math.Max(width, 1), Height = Math.Max(height, 1) };
        }

        private void ScaleImage(Image source, Image destination)
        {
            using (Graphics graphics = Graphics.FromImage(destination))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.SmoothingMode = SmoothingMode.AntiAlias;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.DrawImage(source, 0, 0, destination.Width, destination.Height);
            }
        }

        [HttpPost]
        public ActionResult Create(string path, string type, string name)
        {
            string root = Server.MapPath(ContentPath);

            if (type == "d")
            {
                string dir = (String.IsNullOrEmpty(path)) ? root : root + @"\" + path;
                dir = (String.IsNullOrEmpty(name)) ? dir : dir + @"\" + name;
                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }

            }
            return new EmptyResult();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Upload(string path, string name)
        {

            string root = Server.MapPath(ContentPath);
            string dir = (String.IsNullOrEmpty(path)) ? root : root + @"\" + path;
            dir = (String.IsNullOrEmpty(name)) ? dir : dir + @"\" + name;
            bool isUploaded = false;
            string pathfile = "";
            foreach (string requestname in Request.Files)
            {
                HttpPostedFileBase fileupload = Request.Files[requestname];
                if (fileupload != null)
                {
                    fileupload.SaveAs(dir + fileupload.FileName);
                    pathfile = dir + fileupload.FileName;
                    isUploaded = true;
                }
            }
            if (isUploaded)
            {
                var data = new { name = Path.GetFileName(pathfile), type = "f", size = new FileInfo(pathfile).Length };
                return Json(data, JsonRequestBehavior.AllowGet);
            }

            //ImageUtil image = new ImageUtil(name);
            //string file = image.saveFile(dir);
            //var data = new { name = Path.GetFileName(file), type = "f", size = new FileInfo(file).Length };
            return Json(new { }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Destroy(string path, string name, string type)
        {
            string root = Server.MapPath(ContentPath) ;
            string dir = (String.IsNullOrEmpty(path)) ? root : root + @"\" + path;
            dir = (String.IsNullOrEmpty(name)) ? dir : dir + @"\" + name;
            if (type == "d")
            {
                Directory.Delete(dir, true);
            }
            else new FileInfo(dir).Delete();
            return new EmptyResult();
        }

        private static bool IsDirectory(string path)
        {
            System.IO.FileAttributes fa = System.IO.File.GetAttributes(path);
            bool isDirectory = false;
            if ((fa & FileAttributes.Directory) != 0)
            {
                isDirectory = true;
            }
            return isDirectory;
        }

    }

    public class ImageSize
    {
        public int Height { get; set; }

        public int Width { get; set; }
    }
}
