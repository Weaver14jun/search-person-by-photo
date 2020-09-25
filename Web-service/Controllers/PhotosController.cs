using System;
using System.Collections.Generic;
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
        public ActionResult UserPhotos()
        {
            User user = null;
            using (UserContext db = new UserContext())
            {
                user = db.Users.First(u => u.Email == User.Identity.Name);
            }
            List<Photos> photos = new List<Photos>();
            using (PhotosContext db = new PhotosContext())
            {
                foreach(var item in db.Photos.ToList())
                {
                    if(item.IdUser == user.Id)
                    {
                        photos.Add(item);
                    }
                }
            }
            ViewBag.Photos = photos;
            ViewBag.User = user;
            return View();
        }

        public ActionResult Insert()
        {
            return View();
        }

        //[HttpPost]
        //public ActionResult Insert(string fileName, byte[] imageData)
        //{
        //    User user = null;
        //    using (UserContext db = new UserContext())
        //    {
        //        user = db.Users.First(u => u.Email == User.Identity.Name);
        //    }

        //    using (PhotosContext db = new PhotosContext())
        //    {
        //        db.Photos.Add(new Photos { IdUser = user.Id, FileName = fileName, ImageData = imageData });
        //        db.SaveChanges();
        //    }

        //    return RedirectToAction("Index", "Home");
        //    //return "";
        //}

        [HttpPost]
        public ActionResult UploadFiles(IEnumerable<HttpPostedFileBase> files)
        {
            User user = null;
            using (UserContext db = new UserContext())
            {
                user = db.Users.First(u => u.Email == User.Identity.Name);
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
                using (PhotosContext db = new PhotosContext())
                {
                    db.Photos.Add(new Photos { FileName = photo.FileName, IdUser = photo.IdUser, ImageData = photo.ImageData });
                    db.SaveChanges();
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
        public JsonResult Search(HttpPostedFileBase file)
        {
            User user = null;
            using (UserContext db = new UserContext())
            {
                user = db.Users.First(u => u.Email == User.Identity.Name);
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

            using (UserContext db = new UserContext())
            {
                user = db.Users.First(u => u.Email == User.Identity.Name);
            }
            Photos findedPhoto = new Photos();
            using (PhotosContext db = new PhotosContext())
            {
                findedPhoto = db.Photos.FirstOrDefault(p => p.ImageData == photo.ImageData);
            }
            if (findedPhoto == null)
            {
                return Json("No such user", JsonRequestBehavior.AllowGet);
            }
            User findedUser = null;
            if(findedPhoto != null)
            {
                using (UserContext db = new UserContext())
                {
                    findedUser = db.Users.First(u => u.Id == findedPhoto.IdUser);
                }
                //ViewBag.FindedUserFIO = findedUser.FIO;
               
            }
            if(findedPhoto == null)
            {
                return Json("No such user", JsonRequestBehavior.AllowGet);
            }
            //ViewData["FIO"] = findedUser.FIO;
            //ModelState.AddModelError("", findedUser.FIO);
            //return RedirectToAction("Search", "Photos");
            return Json(findedUser.FIO, JsonRequestBehavior.AllowGet);
            //return View(new { Value = findedUser.FIO });
            //return Content("<p>"+findedUser.FIO+"</p>");

        }

        public ActionResult AjaxDelete()
        {
            var id = Convert.ToInt32(Request["data"]);

            using (PhotosContext db = new PhotosContext())
            {
                var delete = db.Photos.Where(u => u.Id == id).ToList();
                foreach (var item in delete)
                {
                    db.Photos.Remove(item);
                }
                db.SaveChanges();
            }
            if (Request.IsAjaxRequest())
            {
                User user = null;
                using (UserContext db = new UserContext())
                {
                    user = db.Users.First(u => u.FIO == User.Identity.Name);
                }

                //List<Tasks> tasks = new List<Tasks>();
                //using (TasksContext db = new TasksContext())
                //{
                //    var query = db.Tasks.Where(u => u.IdUser == user.Id).ToList();
                //    foreach (var item in query)
                //    {
                //        if (item.Name.Length > 30)
                //        {
                //            string str = "";
                //            string str1 = item.Name;
                //            while (str1.Length > 30)
                //            {
                //                str += str1.Substring(0, 30) + " ";
                //                str1 = str1.Remove(0, 30);
                //            }
                //            str += str1;
                //            item.Name = str;
                //        }
                //        if (item.Description.Length > 30)
                //        {
                //            string str = "";
                //            string str1 = item.Description;
                //            while (str1.Length > 30)
                //            {
                //                str += str1.Substring(0, 30) + " ";
                //                str1 = str1.Remove(0, 30);
                //            }
                //            str += str1;
                //            item.Description = str;
                //        }
                //        tasks.Add(item as Tasks);
                //    }
                //}

                ViewBag.User = user;
                //ViewBag.Tasks = tasks;

                return PartialView("Index");
            }
            return RedirectToAction("Index", "Home");
        }

    }
}