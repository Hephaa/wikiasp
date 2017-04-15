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
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index()
        {
            
            if (GetUserType() != 2)
            {
                return Redirect("/");
            }

            return View(new WikiaModels().Wikias);
        }

        public ActionResult Create()
        {

            if (GetUserType() != 2)
            {
                return Redirect("/");
            }

            return View();
        }

        [HttpPost]
        public ActionResult Add()
        {
            if (GetUserType() != 2)
            {
                return Redirect("/");
            }
            string title = Request.Form["Title"];
            string body = Request.Form["Body"];
           
            //Create the wikia
            var wikiaDb = new WikiaModels();

            Wikias w = new Wikias();
            w.Title = title;

            wikiaDb.Wikias.Add(w);

            

            Wikias wiki = wikiaDb.Wikias.Last();

            var elementDb = new ElementModels();


            elementDb.Element.Add(new Element(title, body, wiki.Id));

            elementDb.SaveChanges();

            Element element = elementDb.Element.Last();

            wiki.IndexElementId = element.Id;

            wikiaDb.SaveChanges();

            return Redirect("/Admin");
        }
        public ActionResult Edit()
        {

            if (GetUserType() != 2)
            {
                return Redirect("/");
            }

            return View();
        }
        public ActionResult Delete()
        {

            if (GetUserType() != 2)
            {
                return Redirect("/");
            }

            return View();
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