using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WelcomeScreen.Models;

namespace WelcomeScreen.Controllers
{
    public class WelcomeVisitHistsController : IntraController
    {
        //private IntranetContext db = new IntranetContext();

        // GET: WelcomeVisitHists
        public ActionResult Index()
        {
            var role = UserRole();
            if (!role.WS_ViewVisit)
            {
                return RedirectToAction("Unauthorized", "Error", new { role.Role, role.UserName});
            }

            var visithists = db.WelcomeVisitHists
                    .Include(vh => vh.GuestVisitHists)
                    .Where(vh => !vh.Deleted) //for teszt
                    .OrderByDescending(vh => vh.ToDT)
                    .ThenByDescending(vh => vh.FromDT)
                    .ThenBy(vh => vh.Visit)
                    .ToList();
            foreach (var vh in visithists)
            {
                vh.GuestVisitHists = vh.GuestVisitHists
                    .OrderByDescending(gvh => gvh.Guest.Rank.Importance)
                    .ThenBy(gvh => gvh.Guest.Fullname)
                    .ToList();
            }
            return View(visithists);
        }

        // GET: WelcomeVisitHists/Details/5
        public ActionResult Details(int? id)
        {
            var role = UserRole();
            if (!role.WS_ViewVisit)
            {
                return RedirectToAction("Unauthorized", "Error", new { role.Role, role.UserName});
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            WelcomeVisitHist welcomeVisitHist = db.WelcomeVisitHists.Find(id);
            if (welcomeVisitHist == null)
            {
                return HttpNotFound();
            }

            welcomeVisitHist.GuestVisitHists = db.WelcomeGuestVisitHists
                .Where(gvh => gvh.VisitHist.Visit_ID == welcomeVisitHist.Visit_ID)
                .OrderByDescending(gvh => gvh.Guest.Rank.Importance)
                .ThenBy(gvh => gvh.Guest.Fullname)
                .ToList();

            return View(welcomeVisitHist);
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
