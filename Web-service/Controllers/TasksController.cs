﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web_service.Controllers
{
    public class TasksController : Controller
    {
        public ActionResult UserTasks()
        {
            return View();
        }


        // GET: Tasks
        //public ActionResult Index()
        //{
          //  return View();
        //}
    }
}