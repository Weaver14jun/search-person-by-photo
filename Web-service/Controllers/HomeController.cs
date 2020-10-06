using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web_service.Models;
using System.Web.Security;
using System.Collections;
using System.Threading.Tasks;
using System.Data.Entity;

namespace Web_service.Controllers
{
    public class HomeController : Controller
    {
        public async Task<ActionResult> Index()
        {
            User user = null;
            using (ApplicationContext db = new ApplicationContext())
            {
                user = await db.Users.FirstOrDefaultAsync(u => u.Email == User.Identity.Name);
                //For users table
                if (user.IsAdmin == 1)
                {
                    ViewBag.userList = await db.Users.ToListAsync();
                }
                else
                {
                    ViewBag.userList = null;
                }
                //
            }

            ViewBag.User = user;
            //ViewBag.Tasks = tasks;
            
            if (Request.IsAjaxRequest())
            {
                return PartialView("Index");
            }
            return View();
        }

        public async Task<ActionResult> AjaxGetUsers()
        {
            List<User> userList = new List<User>();
            using (ApplicationContext db = new ApplicationContext())
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

        public ActionResult Insert()
        {
            return View();
        }

        public async Task<ActionResult> AjaxDelete()
        {
            User user = null;
            using (ApplicationContext db = new ApplicationContext())
            {
                user = await db.Users.FirstOrDefaultAsync(u => u.Email == User.Identity.Name);
                if (user.IsAdmin != 1)
                {
                    return null;
                }
            }

            var id = Convert.ToInt32(Request["data"]);

            using (ApplicationContext db = new ApplicationContext())
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
                using (ApplicationContext db = new ApplicationContext())
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

    }
}