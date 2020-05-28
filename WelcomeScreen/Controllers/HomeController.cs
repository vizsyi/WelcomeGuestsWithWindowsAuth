using WelcomeScreen.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WelcomeScreen.Controllers
{
    public class HomeController : IntraController
    {
        //private IntranetContext db = new IntranetContext();

        public ActionResult Index()
        {
            UserRole();

            DateTime dnow = DateTime.Now;
            //OldVisitsToHist(dnow);//todo: Intraba

            var model = new HomeModel
            {
                welcomeGuests = (from v in db.WelcomeVisits
                                 join gv in db.WelcomeGuestVisits on v.Visit_ID equals gv.Visit.Visit_ID
                                 join g in db.WelcomeGuests on gv.Guest.Guest_ID equals g.Guest_ID
                                 //join r in db.WelcomeRanks on g.Rank.Rank_ID equals r.Rank_ID
                                 where v.FromDT < dnow && !g.Deleted
                                 group g.Guest_ID by g into gl
                                 select gl.Key).OrderByDescending(g => g.Rank.Importance).ThenBy(g => g.Fullname),
                onLineReports = db.OLReports.OrderBy(x => x.Order).ToList()
            };
            model.GuestCount = model.welcomeGuests.Count();

            return View(model);
        }

        public ActionResult About()
        {
            if (User.Identity.IsAuthenticated)
            {
               // User.
            }
            
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
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