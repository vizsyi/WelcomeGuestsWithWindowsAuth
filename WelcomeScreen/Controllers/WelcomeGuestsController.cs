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
    public class WelcomeGuestsController : IntraController
    {
        //private IntranetContext db = new IntranetContext();

        // GET: WelcomeGuests
        public ActionResult Index()
        {
            var role = UserRole();
            if (!role.WS_CreateVisit)
            {
                return RedirectToAction("Unauthorized", "Error", new { role.Role, role.UserName});
            }

            var comps = db.WelcomeCompanies
                .Where(c => !c.Deleted)
                .Include(c => c.Guests)
                .OrderBy(c => c.Company).ToList();

            foreach (var comp in comps)
            {
                comp.Guests = comp.Guests
                                .Where(g => !g.Deleted)
                                .OrderByDescending(g => g.Rank.Importance)
                                .ThenBy(g => g.Fullname).ToList();
            }

            return View(comps);
            //return View(db.WelcomeGuests.ToList());
        }

        // GET: WelcomeGuests/Details/5
        public ActionResult Details(int? id)
        {
            var role = UserRole();
            if (!role.WS_CreateVisit)
            {
                return RedirectToAction("Unauthorized", "Error", new { role.Role, role.UserName});
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            WelcomeGuest welcomeGuest = db.WelcomeGuests.Find(id);
            if (welcomeGuest == null)
            {
                return HttpNotFound();
            }

            return View(welcomeGuest);
        }

        // GET: WelcomeGuests/Create
        public ActionResult Create()
        {
            var role = UserRole();
            if (!role.WS_CreateVisit)
            {
                return RedirectToAction("Unauthorized", "Error", new { role.Role, role.UserName});
            }

            WelcomeGuest guest = new WelcomeGuest();
            FillGuestAssignableLists(guest);

            return View(guest);
        }

        // POST: WelcomeGuests/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Fullname,Sel_Comp_ID,Sel_Rank_ID")] WelcomeGuest welcomeGuest)
        {
            var role = UserRole();
            if (!role.WS_CreateVisit)
            {
                return RedirectToAction("Unauthorized", "Error", new { role.Role, role.UserName});
            }

            welcomeGuest.Company = db.WelcomeCompanies.Find(welcomeGuest.Sel_Comp_ID);
            welcomeGuest.Rank = db.WelcomeRanks.Find(welcomeGuest.Sel_Rank_ID);
            welcomeGuest.Deleted = false;

            ModelState.Clear();
            TryValidateModel(welcomeGuest);
            if (ModelState.IsValid)
            {
                db.WelcomeGuests.Add(welcomeGuest);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            FillGuestAssignableLists(welcomeGuest);
            return View(welcomeGuest);
        }

        // GET: WelcomeGuests/Edit/5
        public ActionResult Edit(int? id)
        {
            var role = UserRole();
            if (!role.WS_CreateVisit)
            {
                return RedirectToAction("Unauthorized", "Error", new { role.Role, role.UserName});
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            WelcomeGuest welcomeGuest = db.WelcomeGuests.Find(id);
            if (welcomeGuest == null)
            {
                return HttpNotFound();
            }

            FillGuestAssignableLists(welcomeGuest);
            welcomeGuest.Sel_Comp_ID = welcomeGuest.Company.Comp_ID;
            welcomeGuest.Sel_Rank_ID = welcomeGuest.Rank.Rank_ID;
            return View(welcomeGuest);
        }

        // POST: WelcomeGuests/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Guest_ID,Fullname,Sel_Comp_ID,Sel_Rank_ID")] WelcomeGuest welcomeGuest)
        {
            var role = UserRole();
            if (!role.WS_CreateVisit)
            {
                return RedirectToAction("Unauthorized", "Error", new { role.Role, role.UserName});
            }

            var model = db.WelcomeGuests.Find(welcomeGuest.Guest_ID);
            if (model == null)
            {
                return HttpNotFound();
            }

            model.Fullname = welcomeGuest.Fullname;
            model.Company = db.WelcomeCompanies.Find(welcomeGuest.Sel_Comp_ID);
            model.Rank = db.WelcomeRanks.Find(welcomeGuest.Sel_Rank_ID);

            ModelState.Clear();
            TryValidateModel(model);
            if (ModelState.IsValid)
            {
                db.Entry(model).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            FillGuestAssignableLists(welcomeGuest);
            return View(welcomeGuest);
        }

        // GET: WelcomeGuests/Delete/5
        public ActionResult Delete(int? id)
        {
            var role = UserRole();
            if (!role.WS_CreateVisit)
            {
                return RedirectToAction("Unauthorized", "Error", new { role.Role, role.UserName});
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            WelcomeGuest welcomeGuest = db.WelcomeGuests.Find(id);
            if (welcomeGuest == null)
            {
                return HttpNotFound();
            }

            return View(welcomeGuest);
        }

        // POST: WelcomeGuests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var role = UserRole();
            if (!role.WS_CreateVisit)
            {
                return RedirectToAction("Unauthorized", "Error", new { role.Role, role.UserName});
            }

            WelcomeGuest welcomeGuest = db.WelcomeGuests.Find(id);
            if (welcomeGuest == null)
            {
                return HttpNotFound();
            }

            if (db.WelcomeGuestVisits.Where(gv => gv.Guest.Guest_ID == id).Count() > 0
                || db.WelcomeGuestVisitHists.Where(gvh => gvh.Guest.Guest_ID == id).Count() > 0)
            {
                welcomeGuest.Deleted = true;
                db.Entry(welcomeGuest).State = EntityState.Modified;
            }
            else
            {
                db.WelcomeGuests.Remove(welcomeGuest);
            }
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        #region Other functions
        /// <summary>
        /// Betölti a lenyílók adattartalmát
        /// </summary>
        /// <param name="guest">az adatmodell, ahová a lenyílók adattartalmát be kell tölteni </param>
        private void FillGuestAssignableLists(WelcomeGuest guest)
        {
            //Választható cégek
            foreach (var comp in db.WelcomeCompanies
                                    .Where(x => !x.Deleted)
                                    .OrderBy(x => x.Company))
                                    //.ToList())
            {
                guest.AssignableCompanies.Add(new SelectListItem()
                {
                    Text = comp.Company,
                    Value = comp.Comp_ID.ToString()
                });
            }

            //Választható rangok
            foreach (var rank in db.WelcomeRanks
                                    .OrderBy(r => r.Importance)
                                    .ThenBy(r => r.Rank))
            {
                guest.AssignableRanks.Add(new SelectListItem()
                {
                    Text = rank.Rank,
                    Value = rank.Rank_ID.ToString()
                });
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        #endregion
    }
}
