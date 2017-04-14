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
            if(!Request.IsAuthenticated)
                return Redirect("/");

            var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            var user = manager.FindById(User.Identity.GetUserId());
            if (user.UserType != 2)
            {
                return Redirect("/");
            }

            return View();
        }
    }
}