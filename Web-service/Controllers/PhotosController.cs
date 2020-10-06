using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Web_service.Models;

namespace Web_service.Controllers
{
    public class PhotosController : Controller
    {

        public async Task<ActionResult> UserPhotos(int? id)
        {
            User user = null;
            if (id == null)
            {
                using (ApplicationContext db = new ApplicationContext())
                {
                    user = await db.Users.FirstOrDefaultAsync(u => u.Email == User.Identity.Name);
                }
            }
            else
            {
                using (ApplicationContext db = new ApplicationContext())
                {
                    user = await db.Users.FirstOrDefaultAsync(u => u.Id == id);
                }
            }
            List<Photos> photos = new List<Photos>();
            using (ApplicationContext db = new ApplicationContext())
            {
                foreach (var item in await db.Photos.ToListAsync())
                {
                    if (item.IdUser == user.Id)
                    {
                        photos.Add(item);
                    }
                }
            }
            ViewBag.Photos = photos;
            ViewBag.User = user;
            return View();
        }

        public ActionResult Insert(int? id)
        {
            ViewData.Model = id;
            ViewBag.UserId = id;
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> UploadFiles(IEnumerable<HttpPostedFileBase> files, int? userId)
        {
            User user = null;
            using (ApplicationContext db = new ApplicationContext())
            {
                if (userId == null)
                {
                    user = await db.Users.FirstOrDefaultAsync(u => u.Email == User.Identity.Name);
                }
                else
                {
                    int? id = userId;
                    user = await db.Users.FirstOrDefaultAsync(u => u.Id == id);
                }
            }
            foreach (var file in files)
            {
                string filePath = Guid.NewGuid() + Path.GetExtension(file.FileName);
                var photo = new Photos();
                //byte[] imageData = null;
                photo.FileName = file.FileName;
                photo.IdUser = user.Id;
                // считываем переданный файл в массив байтов
                using (var binaryReader = new BinaryReader(file.InputStream))
                {
                    photo.ImageData = binaryReader.ReadBytes(file.ContentLength);
                    //imageData = binaryReader.ReadBytes(file.ContentLength);
                }
                using (ApplicationContext db = new ApplicationContext())
                {
                    db.Photos.Add(new Photos { FileName = photo.FileName, IdUser = photo.IdUser, ImageData = photo.ImageData });
                    await db.SaveChangesAsync();
                }

                //file.SaveAs(Path.Combine(Server.MapPath("~/UploadedFiles"), filePath));
                //Here you can write code for save this information in your database if you want
            }
            //return Json("file uploaded successfully");
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Search()
        {
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> Search(HttpPostedFileBase file)
        {
            User user = null;
            using (ApplicationContext db = new ApplicationContext())
            {
                user = await db.Users.FirstOrDefaultAsync(u => u.Email == User.Identity.Name);
            }
            var photo = new Photos();
            //byte[] imageData = null;
            photo.FileName = file.FileName;
            photo.IdUser = user.Id;
            // считываем переданный файл в массив байтов
            using (var binaryReader = new BinaryReader(file.InputStream))
            {
                photo.ImageData = binaryReader.ReadBytes(file.ContentLength);
                //imageData = binaryReader.ReadBytes(file.ContentLength);
            }

            using (ApplicationContext db = new ApplicationContext())
            {
                user = await db.Users.FirstOrDefaultAsync(u => u.Email == User.Identity.Name);
            }
            Photos findedPhoto = new Photos();
            using (ApplicationContext db = new ApplicationContext())
            {
                findedPhoto = db.Photos.FirstOrDefault(p => p.ImageData == photo.ImageData);
            }
            if (findedPhoto == null)
            {
                return Json("No such user", JsonRequestBehavior.AllowGet);
            }
            User findedUser = null;
            if (findedPhoto != null)
            {
                using (ApplicationContext db = new ApplicationContext())
                {
                    findedUser = db.Users.FirstOrDefault(u => u.Id == findedPhoto.IdUser);
                }
                //ViewBag.FindedUserFIO = findedUser.FIO;

            }
            if (findedPhoto == null || findedUser == null)
            {
                return Json("No such user", JsonRequestBehavior.AllowGet);
            }

            return Json(findedUser.FIO, JsonRequestBehavior.AllowGet);

        }

        public async Task<ActionResult> AjaxDelete()
        {
            var id = Convert.ToInt32(Request["data"]);

            using (ApplicationContext db = new ApplicationContext())
            {
                var delete = await db.Photos.Where(u => u.Id == id).ToListAsync();
                foreach (var item in delete)
                {
                    db.Photos.Remove(item);
                }
                await db.SaveChangesAsync();
            }
            if (Request.IsAjaxRequest())
            {
                User user = null;
                using (ApplicationContext db = new ApplicationContext())
                {
                    user = await db.Users.FirstOrDefaultAsync(u => u.FIO == User.Identity.Name);
                }

                ViewBag.User = user;
                //ViewBag.Tasks = tasks;

                return PartialView("Index");
            }
            return RedirectToAction("Index", "Home");
        }

    }
}