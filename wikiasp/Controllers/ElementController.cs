using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
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
            ViewBag.UserType = GetUserType();

            if (element.isParent==1)
            {
                ViewBag.Subs = db.Element.Where(e => e.ParentId == id).ToList();
            }

            return View();
        }

        public ActionResult Edit(int id)
        {
            if (GetUserType() <= 0)
            {
                Session.Add("error", "edit error");
                return Redirect("/");
            }

            var db = new ElementModels();
            Element element = db.Element.Find(id);
            ViewBag.Element = element;

            return View();
        }



        [HttpPost]
        public ActionResult Edit()
        {
            string title = Request.Form["Title"];
            string body = Request.Form["Body"];
            string id = Request.Form["Id"];
            var db = new ElementModels();
            Element e = db.Element.Find(Int32.Parse(id));

            if (e.Title != title || e.Body == body)
            {
                e.Title = title;
                e.Body = body;

                db.SaveChanges();
            }

            

            return Redirect("/wiki/show/" + id);
        }

        public int GetUserType()
        {
            if (!Request.IsAuthenticated) return -1;

            var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            var user = manager.FindById(User.Identity.GetUserId());

            

            return user.UserType;
        }
    }
}