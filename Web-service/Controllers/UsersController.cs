using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Web_service.Models;

namespace Web_service.Controllers
{
    public class UsersController : Controller
    {
        // GET: Users
        public async Task<ActionResult> Index()
        {
            User user = null;
            using (UserContext db = new UserContext())
            {
                user = await db.Users.FirstOrDefaultAsync(u => u.Email == User.Identity.Name);
                //For users table
                if (user.IsAdmin == 1)
                {
                    ViewBag.userList = db.Users.ToList();
                }
                else
                {
                    ViewBag.userList = null;
                }
            }

            ViewBag.User = user;

            if (Request.IsAjaxRequest())
            {
                return PartialView("/Home/Index");
            }
            return View();
        }

        public async Task<ActionResult> AjaxDelete()
        {
            User user = null;
            using (UserContext db = new UserContext())
            {
                user = await db.Users.FirstOrDefaultAsync(u => u.Email == User.Identity.Name);
                if (user.IsAdmin != 1)
                {
                    return null;
                }
            }

            var id = Convert.ToInt32(Request["data"]);

            using (UserContext db = new UserContext())
            {
                var delete = await db.Users.Where(u => u.Id == id).ToListAsync();
                foreach (var item in delete)
                {
                    db.Users.Remove(item);
                }
                await db.SaveChangesAsync();
            }
            if (Request.IsAjaxRequest())
            {
                List<User> userList = new List<User>();
                using (UserContext db = new UserContext())
                {
                    userList = await db.Users.ToListAsync();
                }

                ViewBag.userList = userList;
                //ViewBag.Tasks = tasks;

                return PartialView("Index");
                //return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("Index", "Home");
        }

        public async Task<ActionResult> AjaxGetUsers()
        {
            List<User> userList = new List<User>();
            using (UserContext db = new UserContext())
            {
                userList = await db.Users.ToListAsync();
            }

            ViewBag.userList = userList;
            if (Request.IsAjaxRequest())
            {
                return PartialView("userList");
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<ActionResult> Change(int id)
        {
            User user = null;
            using (UserContext db = new UserContext())
            {
                user = await db.Users.FirstOrDefaultAsync(u => u.Id == id);
            }

            ViewBag.UserData = user;
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Change(int id, RegisterModel registerModel)
        {
            User user = null;
            using (UserContext db = new UserContext())
            {
                user = await db.Users.FirstAsync(u => u.Id == id);
                user.FIO = registerModel.FIO;
                user.Age = registerModel.Age;
                user.Email = registerModel.Login;
                user.Password = registerModel.Password;
                db.SaveChanges();

                user = await db.Users.FirstAsync(u => u.Email == User.Identity.Name);
            }

            if (user.Id != id)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }
    }
}