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
    public class WelcomeRanksController : IntraController
    {
        //private IntranetContext db = new IntranetContext();

        #region Normal WelcomeRanks actions
        // GET: WelcomeRanks
        public ActionResult Index()
        {
            var role = UserRole();
            if (!role.WS_Settings)
            {
                return RedirectToAction("Unauthorized", "Error", new { role.Role, role.UserName});
            }

            return View(db.WelcomeRanks
                .OrderByDescending(r => r.Importance)
                .ThenBy(r => r.Rank)
                .ToList());
        }

        // GET: WelcomeRanks/Create
        public ActionResult Create()
        {
            var role = UserRole();
            if (!role.WS_Settings)
            {
                return RedirectToAction("Unauthorized", "Error", new { role.Role, role.UserName});
            }

            return View();
        }

        // POST: WelcomeRanks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Rank,Importance")] WelcomeRank welcomeRank)
        {
            var role = UserRole();
            if (!role.WS_Settings)
            {
                return RedirectToAction("Unauthorized", "Error", new { role.Role, role.UserName});
            }

            if (ModelState.IsValid)
            {
                db.WelcomeRanks.Add(welcomeRank);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(welcomeRank);
        }

        // GET: WelcomeRanks/Edit/5
        public ActionResult Edit(int? id)
        {
            var role = UserRole();
            if (!role.WS_Settings)
            {
                return RedirectToAction("Unauthorized", "Error", new { role.Role, role.UserName});
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WelcomeRank welcomeRank = db.WelcomeRanks.Find(id);
            if (welcomeRank == null)
            {
                return HttpNotFound();
            }
            return View(welcomeRank);
        }

        // POST: WelcomeRanks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Rank_ID,Rank,Importance")] WelcomeRank welcomeRank)
        {
            var role = UserRole();
            if (!role.WS_Settings)
            {
                return RedirectToAction("Unauthorized", "Error", new { role.Role, role.UserName});
            }

            if (ModelState.IsValid)
            {
                db.Entry(welcomeRank).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(welcomeRank);
        }

        // GET: WelcomeRanks/Delete/5
        public ActionResult Delete(int? id)
        {
            var role = UserRole();
            if (!role.WS_Settings)
            {
                return RedirectToAction("Unauthorized", "Error", new { role.Role, role.UserName});
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            WelcomeRank welcomeRank = db.WelcomeRanks.Find(id);
            if (welcomeRank == null)
            {
                return HttpNotFound();
            }

            welcomeRank.guests = db.WelcomeGuests
                .Where(g => g.Rank.Rank_ID == id)
                .OrderBy(g => g.Company.Company)
                .ThenBy(g => g.Fullname)
                .ToList();

            return View(welcomeRank);
        }

        // POST: WelcomeRanks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var role = UserRole();
            if (!role.WS_Settings)
            {
                return RedirectToAction("Unauthorized", "Error", new { role.Role, role.UserName});
            }

            WelcomeRank welcomeRank = db.WelcomeRanks.Find(id);
            db.WelcomeRanks.Remove(welcomeRank);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        #endregion

        #region Guests' WelcomeRanks actions
        // GET: WelcomeRanks/GREdit/5
        public ActionResult GREdit(int? id)
        {
            var role = UserRole();
            if (!role.WS_Settings)
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

            FillAssignableRanks(welcomeGuest);
            return View(welcomeGuest);
        }

        // POST: WelcomeGuests/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GREdit([Bind(Include = "Guest_ID,Sel_Rank_ID")] WelcomeGuest welcomeGuest)
        {
            var role = UserRole();
            if (!role.WS_Settings)
            {
                return RedirectToAction("Unauthorized", "Error", new { role.Role, role.UserName});
            }

            var model = db.WelcomeGuests.Find(welcomeGuest.Guest_ID);
            if (model == null)
            {
                return HttpNotFound();
            }

            WelcomeRank exrank = model.Rank;
            model.Rank = db.WelcomeRanks.Find(welcomeGuest.Sel_Rank_ID);
            if (model.Rank == null)
            {
                return HttpNotFound();
            }

            ModelState.Clear();
            TryValidateModel(model);
            if (ModelState.IsValid)
            {
                db.Entry(model).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Delete", new { id = exrank.Rank_ID });
            }

            model.Rank = exrank;
            FillAssignableRanks(model);
            return View(model);
        }
        #endregion

        #region Other functions
        /// <summary>
        /// Betölti a Pozíciók lenyíló adattartalmát
        /// </summary>
        /// <param name="welcomeGuest">az adatmodell, ahová a lenyíló adattartalmát be kell tölteni</param>
        /// <param name="rid"></param>Annak a pozíciónak az ID-ja, melyet nem szeretnénk a lenyílóban látni.
        private void FillAssignableRanks(WelcomeGuest welcomeGuest)
        {
            foreach (var rank in db.WelcomeRanks
                            .Where(r => r.Rank_ID != welcomeGuest.Rank.Rank_ID)
                            .OrderBy(r => r.Importance)
                            .ThenBy(r => r.Rank))
            {
                welcomeGuest.AssignableRanks.Add(new SelectListItem()
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
