using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WelcomeScreen.Models;

namespace WelcomeScreen.Controllers
{
    public class IntraController : Controller
    {
        protected IntranetContext db = new IntranetContext();

        protected IntraRole intrarole = new IntraRole();

        protected IntraRole UserRole()
        {
            //public string name = User.Identity.Name;
            //private IntraRole role = new IntraRole(User.Identity.Name);
            IntraRole role = new IntraRole(User.Identity.Name);
            IEnumerable<IntraRole> roles = db.IntraRoles;
            foreach (IntraRole r in roles)
            {
                if (r.Role == "Informatikai vezetők")
                {
                    role.Add(r);
                }
            }
            ViewBag.role = role;
            return role;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}