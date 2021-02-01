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
            Order order = null;
            using (ApplicationContext db = new ApplicationContext())
            {
                ViewBag.orderList = await db.Orders.ToListAsync();
            }

            ViewBag.Order = order;

            if (Request.IsAjaxRequest())
            {
                return PartialView("Index");
            }
            return View();
        }

        public ActionResult Insert()
        {
            return View();
        }

        public ActionResult Change()
        {
            return View();
        }

        public async Task<ActionResult> AjaxFind()
        {
            DateTime startDate = DateTime.MinValue;
            DateTime endDate = DateTime.MaxValue;
            try
            {
                startDate = Convert.ToDateTime(Request["dateTimeStart"]);
                endDate = Convert.ToDateTime(Request["dateTimeEnd"]);
            }
            catch { return new HttpStatusCodeResult(400); }

            List<Order> orders = new List<Order>();

            using (ApplicationContext db = new ApplicationContext())
            {
                orders = await db.Orders.Where(x => (x.Date >= startDate) && (x.Date <= endDate)).ToListAsync();
            }

            if (Request.IsAjaxRequest())
            {

                ViewBag.orderList = orders;

                return PartialView("Index");
            }
            return RedirectToAction("Index", "Home");

        }

        public async Task<ActionResult> AjaxDelete()
        {
            var id = Convert.ToInt32(Request["data"]);


            using (ApplicationContext db = new ApplicationContext())
            {
                var delete = await db.Orders.Where(u => u.Id == id).ToListAsync();
                foreach (var item in delete)
                {
                    db.Orders.Remove(item);
                }
                await db.SaveChangesAsync();
            }
            if (Request.IsAjaxRequest())
            {

                List<Order> orderList = new List<Order>();
                using (ApplicationContext db = new ApplicationContext())
                {
                    orderList = await db.Orders.ToListAsync();
                }

                ViewBag.orderList = orderList;

                return PartialView("Index");
            }
            return RedirectToAction("Index", "Home");
        }

    }
}