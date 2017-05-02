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

        public ActionResult Users()
        {
            if (GetUserType() != 2)
            {
                return Redirect("/");
            }

            return View();
        }

        [HttpPost]
        public ActionResult Save()
        {
            string id = Request.Form["id"];
            int userType = Convert.ToInt32(Request.Form["userType"]);

            var db = new ApplicationDbContext();

            ApplicationUser user =  db.Users.Find(id);

            user.UserType = userType;

            db.SaveChanges();

            return Redirect("/admin/users");
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
            w.IndexElementId = -1;
            w.CreationDate = DateTime.Now;
            w.LastUpdateDate = DateTime.Now;

            wikiaDb.Wikias.Add(w);

            wikiaDb.SaveChanges();
            

            var elementDb = new ElementModels();

            Element element = new Element(title, body, w.Id);
            element.CreationDate = DateTime.Now;
            element.LastUpdateDate = DateTime.Now;
            element.ParentId = -1;

            elementDb.Element.Add(element);

            elementDb.SaveChanges();

            w.IndexElementId = element.Id;

            wikiaDb.SaveChanges();

            return Redirect("/Admin");
        }
        public ActionResult Delete(int id)
        {

            if (GetUserType() != 2)
            {
                return Redirect("/");
            }
            var wikiDb = new WikiaModels();
            Wikias wiki = wikiDb.Wikias.Find(id);

            wikiDb.Wikias.Remove(wiki);

            wikiDb.SaveChanges();


            var db = new ElementModels();
            List<Element> list = db.Element.Where(x => x.WikiaId == id).ToList();

            foreach (Element e in list)
            {
                db.Element.Remove(e);
            }
            db.SaveChanges();
            


            return Redirect("/admin/");
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