using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web_service.Models;

namespace Web_service.Controllers
{
    public class UsersController : Controller
    {
        // GET: Users
        public ActionResult Index()
        {
            User user = null;
            using (UserContext db = new UserContext())
            {
                user = db.Users.FirstOrDefault(u => u.Email == User.Identity.Name);
                //For users table
                if (user.IsAdmin == 1)
                {
                    ViewBag.userList = db.Users.ToList();
                }
                else
                {
                    ViewBag.userList = null;
                }
                //
            }

            List<Tasks> tasks = new List<Tasks>();
            using (TasksContext db = new TasksContext())
            {
                var query = db.Tasks.Where(u => u.IdUser == user.Id).ToList();
                foreach (var item in query)
                {
                    if (item.Name.Length > 30)
                    {
                        string str = "";
                        string str1 = item.Name;
                        while (str1.Length > 30)
                        {
                            str += str1.Substring(0, 30) + " ";
                            str1 = str1.Remove(0, 30);
                        }
                        str += str1;
                        item.Name = str;
                    }
                    if (item.Description.Length > 30)
                    {
                        string str = "";
                        string str1 = item.Description;
                        while (str1.Length > 30)
                        {
                            str += str1.Substring(0, 30) + " ";
                            str1 = str1.Remove(0, 30);
                        }
                        str += str1;
                        item.Description = str;
                    }
                    tasks.Add(item as Tasks);
                }
            }

            ViewBag.User = user;
            ViewBag.Tasks = tasks;


            if (Request.IsAjaxRequest())
            {
                return PartialView("/Home/Index");
            }
            return View();
        }

        public ActionResult AjaxDelete()
        {
            User user = null;
            using (UserContext db = new UserContext())
            {
                user = db.Users.First(u => u.Email == User.Identity.Name);
                if (user.IsAdmin != 1)
                {
                    return null;
                }
            }

            var id = Convert.ToInt32(Request["data"]);

            using (UserContext db = new UserContext())
            {
                var delete = db.Users.Where(u => u.Id == id).ToList();
                foreach (var item in delete)
                {
                    db.Users.Remove(item);
                }
                db.SaveChanges();
            }
            if (Request.IsAjaxRequest())
            {

                //List<Tasks> tasks = new List<Tasks>();
                //using (UserContext db = new UserContext())
                //{
                //    var query = user.;//db.Users.Where(u => u.Id == user.Id).ToList();
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

                List<User> userList = new List<User>();
                using (UserContext db = new UserContext())
                {
                    userList = db.Users.ToList();
                }

                ViewBag.userList = userList;
                //ViewBag.Tasks = tasks;

                return PartialView("Index");
                //return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("Index", "Home");
        }

        public ActionResult AjaxGetUsers()
        {
            List<User> userList = new List<User>();
            using (UserContext db = new UserContext())
            {
                userList = db.Users.ToList();
            }

            ViewBag.userList = userList;
            if (Request.IsAjaxRequest())
            {
                return PartialView("userList");
            }
            return RedirectToAction("Index", "Home");
        }
    }
}