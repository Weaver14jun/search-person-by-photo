using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Web_service.Models;

namespace Web_service.Controllers
{
    public class AccountController : Controller
    {
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                // поиск пользователя в бд
                User user = null;
                using (ApplicationContext db = new ApplicationContext())
                {
                    user = await db.Users.FirstOrDefaultAsync(u => u.Email == model.Login && u.Password == model.Password);

                }
                if (user != null)
                {
                    FormsAuthentication.SetAuthCookie(model.Login, true);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Пользователя с таким логином и паролем нет");
                }
            }

            return View(model);
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                User user = null;
                using (ApplicationContext db = new ApplicationContext())
                {
                    user = await db.Users.FirstOrDefaultAsync(u => u.Email == model.Login);
                }
                if (user == null)
                {
                    // создаем нового пользователя
                    using (ApplicationContext db = new ApplicationContext())
                    {
                        db.Users.Add(new User { Email = model.Login, FIO = model.FIO, Password = model.Password, Age = model.Age });
                        await db.SaveChangesAsync();

                        user = await db.Users.Where(u => u.Email == model.Login && u.Password == model.Password).FirstOrDefaultAsync();
                    }
                    // если пользователь удачно добавлен в бд
                    if (user != null)
                    {
                        FormsAuthentication.SetAuthCookie(model.Login, true);
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Пользователь с таким логином уже существует");
                }
            }

            return View(model);
        }
        public ActionResult Logoff()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }
    }
}