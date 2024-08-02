using Data_Access_Layer.Interfaces;
using Microsoft.AspNetCore.Mvc;
using PicciLeaksModels;
using System.Text.RegularExpressions;
using static System.Net.Mime.MediaTypeNames;

namespace PicciLeaks.Controllers
{
    public class GalleryController : Controller
    {
        private IDbActions<Picture> sqlDb = null;
        private IWebHostEnvironment webHost = null;

        public GalleryController(IDbActions<Picture> sqlDb, IWebHostEnvironment webHost)
        {
            this.sqlDb = sqlDb;
            this.webHost = webHost;
        }

        [HttpGet]
        public IActionResult Index()
        {
            List<Picture> gallery = sqlDb.GetAll();
            ViewData["Page"] = "Gallery";
            return View("gallery", gallery);
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewData["Page"] = "Upload";
            return View("upload");
        }

        [HttpPost]
        public async Task<IActionResult> Create(Picture photo, IFormFile fileToServer)
        {
            photo.FileName = fileToServer.FileName;
            photo.FileSize = (int)fileToServer.Length;
            photo.LoadedAt = DateTime.Now;
            string fullPath = Path.Combine(webHost.WebRootPath, "images", photo.FileName);
            using(FileStream fs = new FileStream(fullPath, FileMode.Create, FileAccess.Write))
            {
                fileToServer.OpenReadStream().CopyTo(fs);
            }
            photo.FilePath = Path.Combine("images", photo.FileName);
            await sqlDb.Add(photo);
            return RedirectToAction();
        }

        [HttpGet]
        public async Task<List<Picture>> Search(string lookingFor)
        {
            string regEx = lookingFor;
            if (!string.IsNullOrEmpty(regEx))
            {
                Regex rg = new Regex("^" + regEx.Replace(".", "\\.").Replace("*", ".*") + "$", RegexOptions.IgnoreCase);
                List<Picture> gal = sqlDb.GetAll();
                List<Picture> filtered = new List<Picture>();
                foreach (Picture item in gal)
                {
                    if (rg.IsMatch(item.FileName))
                    {
                        filtered.Add(item);
                    }
                }
                return filtered;
            }
            return null;
        }

        public async Task<FileResult> downloadPicture(string filepath)
        {
            string filePathToDownload = Path.Combine(webHost.WebRootPath, filepath);
            byte[] binaryPicture = await System.IO.File.ReadAllBytesAsync(filePathToDownload);
            return File(binaryPicture, Application.Octet, Path.GetFileName(filePathToDownload));
        }

        public Picture ShowPicture(int id)
        {
            return sqlDb.GetById(id);
        }
    }
}
