using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web_service.Models;
using System.Web.Security;
using System.Collections;

namespace Web_service.Controllers
{
    public class HomeController : Controller
    {
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
                return PartialView("Index");
            }
            return View();
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

        public ActionResult AjaxFind()
        {
            var condition = Convert.ToInt32(Request["sel"]);
            var find = Request["data"];

            User user = null;
            using (UserContext db = new UserContext())
            {
                user = db.Users.First(u => u.FIO == User.Identity.Name);
            }

            List<Tasks> tasks = new List<Tasks>();
            using (TasksContext db = new TasksContext())
            {
                var query = db.Tasks.Where(u => u.IdUser == user.Id).ToList();
                foreach (var item in query)
                {
                    if (find == "")
                    {
                        if (condition == 1)
                            tasks.Add(item as Tasks);
                        if (condition == 2 && item.condition == false)
                            tasks.Add(item as Tasks);
                        if (condition == 3 && item.condition == true)
                            tasks.Add(item as Tasks);
                    }
                    else
                    {
                        if (condition == 1 && item.Name == find)
                            tasks.Add(item as Tasks);
                        if (condition == 2 && item.condition == false && item.Name == find)
                            tasks.Add(item as Tasks);
                        if (condition == 3 && item.condition == true && item.Name == find)
                            tasks.Add(item as Tasks);
                    }
                }
            }

            foreach (var item in tasks)
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
            }

            ViewBag.User = user;
            ViewBag.Tasks = tasks;

            return View();
        }

        public ActionResult Insert()
        {
            var t = View();
            return View();
        }

        [HttpPost]
        public ActionResult Insert(string name, string description)
        {
            User user = null;
            using (UserContext db = new UserContext())
            {
                user = db.Users.First(u => u.Email == User.Identity.Name);
            }

            using (TasksContext db = new TasksContext())
            {
                db.Tasks.Add(new Tasks { Name = name, Description = description, IdUser = user.Id });
                db.SaveChanges();
            }

            return RedirectToAction("Index", "Home");
            //return "";
        }
        //добавление коментария через аджакс
        public ActionResult AjaxInsertComment()
        { 
            return View();
        }

        public ActionResult AjaxInsertComments()
        {
            int id = Convert.ToInt32(Session["id"]);
            string description = Request["data"];
            using (CommentContext db = new CommentContext())
            {
                db.Comment.Add(new Comment { Description = description, IdUser = id });
                db.SaveChanges();
            }

            return RedirectToAction("Change", "Home", new { id = id });
            //return "";
        }

        public ActionResult Delete(int id)
        {
            using (TasksContext db = new TasksContext())
            {
                var delete = db.Tasks.Where(u => u.Id == id).ToList();
                foreach(var item in delete)
                {
                    db.Tasks.Remove(item);
                }
                db.SaveChanges();
            }

            return RedirectToAction("Index", "Home");
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

        //public ActionResult AjaxDelete()
        //{
        //    var id = Convert.ToInt32(Request["data"]);

        //    using (TasksContext db = new TasksContext())
        //    {
        //        var delete = db.Tasks.Where(u => u.Id == id).ToList();
        //        foreach (var item in delete)
        //        {
        //            db.Tasks.Remove(item);
        //        }
        //        db.SaveChanges();
        //    }
        //    if (Request.IsAjaxRequest())
        //    {
        //        User user = null;
        //        using (UserContext db = new UserContext())
        //        {
        //            user = db.Users.First(u => u.Email == User.Identity.Name);
        //        }

        //        List<Tasks> tasks = new List<Tasks>();
        //        using (TasksContext db = new TasksContext())
        //        {
        //            var query = db.Tasks.Where(u => u.IdUser == user.Id).ToList();
        //            foreach (var item in query)
        //            {
        //                if (item.Name.Length > 30)
        //                {
        //                    string str = "";
        //                    string str1 = item.Name;
        //                    while (str1.Length > 30)
        //                    {
        //                        str += str1.Substring(0, 30) + " ";
        //                        str1 = str1.Remove(0, 30);
        //                    }
        //                    str += str1;
        //                    item.Name = str;
        //                }
        //                if (item.Description.Length > 30)
        //                {
        //                    string str = "";
        //                    string str1 = item.Description;
        //                    while (str1.Length > 30)
        //                    {
        //                        str += str1.Substring(0, 30) + " ";
        //                        str1 = str1.Remove(0, 30);
        //                    }
        //                    str += str1;
        //                    item.Description = str;
        //                }
        //                tasks.Add(item as Tasks);
        //            }
        //        }

        //        ViewBag.User = user;
        //        ViewBag.Tasks = tasks;

        //        return PartialView("Index");
        //    }
        //    return RedirectToAction("Index", "Home");
        //}

        public ActionResult AjaxDeleteComment()
        {
            var id = Convert.ToInt32(Request["data"]);
            var idTasks = Convert.ToInt32(Request["idTask"]);

            using (CommentContext db = new CommentContext())
            {
                var delete = db.Comment.Where(u => u.Id == id).ToList();
                foreach(var item in delete)
                {
                    db.Comment.Remove(item);
                }
                db.SaveChanges();
            }

            return RedirectToAction("Change", "Home", new { id = idTasks });
        }

        public ActionResult AjaxChangeComment()
        {
            var id = Convert.ToInt32(Request["data"]);
            Session["idComment"] = id;
            using (CommentContext db = new CommentContext())
            {
                var request = db.Comment.Where(u => u.Id == id).ToList();
                foreach (var item in request)
                {
                    ViewBag.Comment = item as Comment;
                }
            }
            return View();
        }

        public ActionResult AjaxChangeComments()
        {
            var id = Convert.ToInt32(Session["id"]);
            var idComment = Convert.ToInt32(Session["idComment"]);
            var value = Request["data"];

            using (CommentContext db = new CommentContext())
            {
                var request = db.Comment.Find(idComment);
                request.Description = value;
                if (TryUpdateModel(request))
                {
                    db.SaveChanges();
                }
            }

            return RedirectToAction("Change", "Home", new { id = id });
        }

        [HttpGet]
        public ActionResult Change(int id)
        {
            Session["id"] = id;
            using (TasksContext db = new TasksContext())
            {
                var query = db.Tasks.Where(u => u.Id == id).ToList();
                foreach (var item in query)
                {
                    ViewBag.Tasks = item as Tasks;
                }
            }

            List<Comment> comment = new List<Comment>();
            using (CommentContext db = new CommentContext())
            {
                var query = db.Comment.Where(u => u.IdUser == id).ToList();
                foreach (var item in query)
                {
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

                    comment.Add(item as Comment);
                }
            }
            ViewBag.Comment = comment;
                //ViewBag.TasksId = id;
                return View();
        }

        public ActionResult AjaxUpdateCondition()
        {
            var id = Convert.ToInt32(Request["data"]);

            using (TasksContext db = new TasksContext())
            {
                var update = db.Tasks.Find(id);
                if (update.condition)
                    update.condition = false;
                else
                    update.condition = true;
                if (TryUpdateModel(update))
                {
                    db.SaveChanges();
                }
                //db.Tasks.Update(task);
                //db.SaveChanges();
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public ActionResult Change(Tasks task)
        {
            using (TasksContext db = new TasksContext())
            {
                var update = db.Tasks.Find(task.Id);
                update.Name = task.Name;
                update.Description = task.Description;
                if(TryUpdateModel(update))
                {
                    db.SaveChanges();
                }
                //db.Tasks.Update(task);
                //db.SaveChanges();
            }

            return RedirectToAction("Index", "Home");
            //return "";
        }
    }
}