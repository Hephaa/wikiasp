using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using wikiasp.Models;

namespace wikiasp.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var db = new WikiaModels();
            ViewBag.WikiList = db.Wikias.ToList();
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}