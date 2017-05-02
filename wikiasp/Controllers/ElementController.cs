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

            if(element == null) return Redirect("/");

            if (element.isParent == 1)
            {
                ViewBag.Subs = db.Element.Where(e => e.ParentId == id).ToList();
            }
            if (element.ParentId > 0)
            {
                ViewBag.Siblings = db.Element.Where(e => e.ParentId == element.ParentId && e.Id != id).ToList();

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

            if (e.Title != title || e.Body != body)
            {
                //Element update
                e.Title = title;
                e.Body = body;
                e.LastUpdateDate = DateTime.Now;

                //wiki last update
                var wikiDb = new WikiaModels();
                Wikias wiki = wikiDb.Wikias.Where(x => x.Id == e.WikiaId).ToArray()[0];
                wiki.LastUpdateDate = DateTime.Now;

                wikiDb.SaveChanges();


                db.SaveChanges();
            }



            return Redirect("/wiki/show/" + id);
        }

        public ActionResult Create(int id)
        {
            if (GetUserType() <= 0)
            {
                Session.Add("error", "edit error");
                return Redirect("/");
            }

            var db = new ElementModels();

            ViewBag.Parent = db.Element.Find(id);

            return View();
        }
        [HttpPost]
        public ActionResult Add()
        {
            if (GetUserType() <= 0)
            {
                Session.Add("error", "edit error");
                return Redirect("/");
            }

            String title = Request.Form["Title"];
            String body = Request.Form["Body"];
            int parentId = Convert.ToInt32(Request.Form["parentId"]);
            int wikiaId = Convert.ToInt32(Request.Form["wikiaId"]);

            var db = new ElementModels();

            Element parent = db.Element.Find(parentId);

            parent.isParent = 1;

            Element element = new Element(title, body, wikiaId);
            element.ParentId = parentId;
            element.CreationDate = DateTime.Now;
            element.LastUpdateDate = DateTime.Now;
            element.isParent = 0;

            db.Element.Add(element);

            db.SaveChanges();

            return Redirect("/wiki/show/" + element.Id);
        }
        
        public ActionResult Delete(int id)
        {
            var db = new ElementModels();
            Element element = db.Element.Find(id);
            int parent = element.ParentId;

            deleteElement(db, id, true);


            return Redirect("/wiki/show/" + parent);
        }

        public void deleteElement(ElementModels db, int id, bool isRecursive)
        {
            Element e = db.Element.Find(id);
            if (isRecursive && e.isParent==1)
            {
                List<Element> subs = db.Element.Where(x => x.ParentId == id).ToList();
                foreach (Element sub in subs)
                {
                    deleteElement(db, sub.Id, isRecursive);
                }
            }

            db.Element.Remove(e);

            db.SaveChanges();

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