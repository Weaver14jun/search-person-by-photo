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
    public class OrdersController : Controller
    {
        [HttpGet]
        public async Task<ActionResult> Index()
        {
            Order order = null;
            using (ApplicationContext db = new ApplicationContext())
            {
                ViewBag.userList = await db.Orders.ToListAsync();
            }

            ViewBag.User = order;
            //ViewBag.Tasks = tasks;

            if (Request.IsAjaxRequest())
            {
                return PartialView("Index");
            }
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Insert(int? sum, DateTime? dateTime)
        {

            if(dateTime == null || sum == null)
            {
                return new HttpStatusCodeResult(400);
            }
            int temp2 = Convert.ToInt32(sum);
            DateTime temp = Convert.ToDateTime(dateTime);
            using (ApplicationContext db = new ApplicationContext())
            {
                db.Orders.Add(new Order { Sum = temp2, Date = temp });
                await db.SaveChangesAsync();
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<ActionResult> Change(int id, int? sum, DateTime? dateTime)
        {
            if (dateTime == null || sum == null)
            {
                return new HttpStatusCodeResult(400);
            }
            int temp2 = Convert.ToInt32(sum);
            DateTime temp = Convert.ToDateTime(dateTime);
            Session["id"] = id;
            using (ApplicationContext db = new ApplicationContext())
            {
                var query = await db.Orders.Where(u => u.Id == id).ToListAsync();
                foreach (var item in query)
                {
                    ViewBag.Orders = item as Order;
                }
            }

            using (ApplicationContext db = new ApplicationContext())
            {
                var changingOrder = await db.Orders.FirstOrDefaultAsync(x => x.Id == id);
                changingOrder.Sum = temp2;
                changingOrder.Date = temp;
                await db.SaveChangesAsync();
            }

            return RedirectToAction("Index", "Home");
        }

        public ActionResult Insert()
        {
            return View();
        }
        public ActionResult Change()
        {
            return View();
        }
        public ActionResult Logoff()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }
    }
}