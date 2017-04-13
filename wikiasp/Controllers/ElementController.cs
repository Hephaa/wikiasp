using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using wikiasp.Models;

namespace wikiasp.Controllers
{
    public class ElementController : Controller
    {
        // GET: Element
        public ActionResult Show(int id)
        {
            var db = new ElementModels();
            Element element = db.Element.Find(id);
            ViewBag.Element = element;
            return View();
        }
    }
}