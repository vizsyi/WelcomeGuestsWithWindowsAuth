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
    public class WelcomeVisitsController : IntraController
    {
        //private IntranetContext db = new IntranetContext();

        #region Controlling Visits
        // GET: welcomeVisits
        public ActionResult Index0() //todo: törölhető
        {
            UserRole();

            DateTime dnow = DateTime.Now;
            OldVisitsToHist(dnow);

            List<WelcomeGuest> guests = (from v in db.WelcomeVisits
                                         join gv in db.WelcomeGuestVisits on v.Visit_ID equals gv.Visit.Visit_ID
                                         join g in db.WelcomeGuests on gv.Guest.Guest_ID equals g.Guest_ID
                                         //join r in db.WelcomeRanks on g.Rank.Rank_ID equals r.Rank_ID
                                         where v.FromDT < dnow && !g.Deleted
                                         group g.Guest_ID by g into gl
                                         select gl.Key).OrderByDescending(g => g.Rank.Importance).ThenBy(g => g.Fullname)
                                    .ToList();

            return View(guests);
        }

        // GET: welcomeVisits
        public ActionResult Index()
        {
            var role = UserRole();
            if (!role.WS_ViewVisit)
            {
                return RedirectToAction("Unauthorized", "Error", new { role.Role, role.UserName});
            }

            DateTime dnow = DateTime.Now;
            OldVisitsToHist(dnow);

            var visits = db.WelcomeVisits
                .Include(v => v.GuestVisits)
                .OrderBy(v => v.FromDT)
                .ThenBy(v => v.ToDT)
                .ThenBy(v => v.Visit)
                .ToList();

            foreach (var v in visits)
            {
                v.GuestVisits = v.GuestVisits
                    //.Where(gv => !gv.Guest.Deleted)
                    .OrderBy(gv => gv.Guest.Deleted)
                    .ThenByDescending(gv => gv.Guest.Rank.Importance)
                    .ThenBy(gv => gv.Guest.Fullname)
                    .ToList();
                v.recent = v.FromDT < dnow;
            }

            return View(visits);
        }

        // GET: welcomeVisits/Details/5
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

            WelcomeVisit welcomeVisit = db.WelcomeVisits.Find(id);
            if (welcomeVisit == null)
            {
                return HttpNotFound();
            }

            welcomeVisit.GuestVisits = welcomeVisit.GuestVisits
                        .OrderBy(gv => gv.Guest.Deleted)
                        .ThenByDescending(gv => gv.Guest.Rank.Importance)
                        .ThenBy(gv => gv.Guest.Fullname)
                        .ToList();
            return View(welcomeVisit);
        }

        // GET: welcomeVisits/Create
        public ActionResult Create()
        {
            var role = UserRole();
            if (!role.WS_CreateVisit)
            {
                return RedirectToAction("Unauthorized", "Error", new { role.Role, role.UserName});
            }

            WelcomeVisit visit = new WelcomeVisit();
            FillAssignableCompanies(visit);
            visit.FromDate = DateTime.Today.AddDays(1);
            visit.FromTime = new TimeSpan(7, 29, 11);
            visit.ToDate = visit.FromDate;
            visit.ToTime = new TimeSpan(19, 31, 13);

            return View(visit);
        }

        // POST: welcomeVisits/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Visit,Sel_Comp_ID,FromDate,FromTime,ToDate,ToTime")] WelcomeVisit welcomeVisit)
        {
            var role = UserRole();
            if (!role.WS_CreateVisit)
            {
                return RedirectToAction("Unauthorized", "Error", new { role.Role, role.UserName});
            }

            welcomeVisit.Company = db.WelcomeCompanies.Find(welcomeVisit.Sel_Comp_ID);
            if (welcomeVisit.Company == null)
            {
                return HttpNotFound();
            }

            DateTime now;
            welcomeVisit.SetDT();
            welcomeVisit.CreatingDT = now = DateTime.Now;

            ValidateVisitTime(welcomeVisit, now);
            if (ModelState.IsValid)
            {
                db.WelcomeVisits.Add(welcomeVisit);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            FillAssignableCompanies(welcomeVisit);
            return View(welcomeVisit);
        }

        // GET: welcomeVisits/Edit/5
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
            WelcomeVisit welcomeVisit = db.WelcomeVisits.Find(id);
            if (welcomeVisit == null)
            {
                return HttpNotFound();
            }

            FillAssignableCompanies(welcomeVisit);
            welcomeVisit.SetParts();
            welcomeVisit.Sel_Comp_ID = welcomeVisit.Company.Comp_ID;
            return View(welcomeVisit);
        }

        // POST: welcomeVisits/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Visit_ID,Visit,Sel_Comp_ID,FromDate,FromTime,ToDate,ToTime")] WelcomeVisit welcomeVisit)
        {
            var role = UserRole();
            if (!role.WS_CreateVisit)
            {
                return RedirectToAction("Unauthorized", "Error", new { role.Role, role.UserName});
            }

            var model = db.WelcomeVisits.Find(welcomeVisit.Visit_ID);
            if (model == null)
            {
                return HttpNotFound();
            }

            model.Company = db.WelcomeCompanies.Find(welcomeVisit.Sel_Comp_ID);
            if (model.Company == null)
            {
                return HttpNotFound();
            }

            model.Visit = welcomeVisit.Visit;
            model.FromDate = welcomeVisit.FromDate;
            model.FromTime = welcomeVisit.FromTime;
            model.ToDate = welcomeVisit.ToDate;
            model.ToTime = welcomeVisit.ToTime;
            model.SetDT();

            ModelState.Clear();
            ValidateVisitTime(model, DateTime.Now);
            TryValidateModel(model);
            if (ModelState.IsValid)
            {
                db.Entry(model).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            FillAssignableCompanies(model);
            return View(model);
        }

        // GET: welcomeVisits/Delete/5
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
            WelcomeVisit welcomeVisit = db.WelcomeVisits.Find(id);
            if (welcomeVisit == null)
            {
                return HttpNotFound();
            }
            return View(welcomeVisit);
        }

        // POST: welcomeVisits/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var role = UserRole();
            if (!role.WS_CreateVisit)
            {
                return RedirectToAction("Unauthorized", "Error", new { role.Role, role.UserName});
            }

            WelcomeVisit welcomeVisit = db.WelcomeVisits.Find(id);
            if (welcomeVisit == null)
            {
                return HttpNotFound();
            }

            DateTime dnow = DateTime.Now;

            double hours = (dnow - welcomeVisit.CreatingDT).TotalHours;

            if (hours > 24 || welcomeVisit.FromDT < dnow) //Ebben az esetben a histbe mentődnek az adatok.
            {
                WelcomeVisitHist vishist = new WelcomeVisitHist
                {
                    Visit_ID = welcomeVisit.Visit_ID,
                    Visit = welcomeVisit.Visit,
                    FromDT = welcomeVisit.FromDT,
                    ToDT = welcomeVisit.ToDT,
                    Company = welcomeVisit.Company,
                    CreatingDT = welcomeVisit.CreatingDT,
                    Deleted = true
                };
                db.WelcomeVisitHists.Add(vishist);

                WelcomeGuestVisitHist gvhist;
                foreach (WelcomeGuestVisit ogv in welcomeVisit.GuestVisits)
                {
                    if (!ogv.Guest.Deleted)
                    {
                        gvhist = new WelcomeGuestVisitHist
                        {
                            Guest = ogv.Guest,
                            VisitHist = vishist
                        };
                        db.WelcomeGuestVisitHists.Add(gvhist);
                    }
                }
            }

            db.WelcomeVisits.Remove(welcomeVisit); 
            db.SaveChanges();

            return RedirectToAction("Index");
        }
        #endregion

        #region Controlling GuestVisits
        // GET: welcomeGuestVisits/GuestCreate/5
        public ActionResult GuestCreate(int? id)
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

            WelcomeVisit welcomeVisit = db.WelcomeVisits.Find(id);
            if (welcomeVisit == null)
            {
                return HttpNotFound();
            }

            WelcomeGuestVisit guestVisit = new WelcomeGuestVisit
            {
                Visit = welcomeVisit
            };

            guestVisit.Visit_ID = (int)id;

            FillAssignableGuests(guestVisit);
            return View(guestVisit);
        }

        // POST: welcomeGuestVisits/GuestCreate/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GuestCreate([Bind(Include = "Visit_ID,Sel_Guest_ID")] WelcomeGuestVisit guestVisit)
        {
            var role = UserRole();
            if (!role.WS_CreateVisit)
            {
                return RedirectToAction("Unauthorized", "Error", new { role.Role, role.UserName});
            }

            guestVisit.Visit = db.WelcomeVisits.Find(guestVisit.Visit_ID);
            guestVisit.Guest = db.WelcomeGuests.Find(guestVisit.Sel_Guest_ID);
            if (guestVisit.Visit == null || guestVisit.Guest == null)
            {
                return HttpNotFound();
            }

            //Csak akkor hozza létre, ha a látogatáshoz még nem kapcsolódik a vendég (felhasználó megnyomta a frissítés gombot).
            if (db.WelcomeGuestVisits.Where(gv => gv.Visit.Visit_ID == guestVisit.Visit_ID && gv.Guest.Guest_ID == guestVisit.Sel_Guest_ID).Count() == 0)
            {
                ModelState.Clear();
                TryValidateModel(guestVisit);
                if (ModelState.IsValid)
                {
                    db.WelcomeGuestVisits.Add(guestVisit);
                    db.SaveChanges();
                }
            }

            FillAssignableGuests(guestVisit);
            return View(guestVisit);
        }

        // GET: welcomeGuestVisits/GuestEdit/5
        public ActionResult GuestEdit(int? id)
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
            WelcomeGuestVisit guestVisit = db.WelcomeGuestVisits.Find(id);
            if (guestVisit == null)
            {
                return HttpNotFound();
            }

            guestVisit.Visit_ID =  guestVisit.Visit.Visit_ID;
            guestVisit.Sel_Guest_ID = guestVisit.Guest.Guest_ID;
            guestVisit.AssignableGuests.Add(new SelectListItem()
            {
                Text = guestVisit.Guest.Fullname,
                Value = guestVisit.Sel_Guest_ID.ToString()
            });
            FillAssignableGuests(guestVisit);
            return View(guestVisit);
        }

        // POST: welcomeGuestsVisits/GuestEdit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GuestEdit([Bind(Include = "GV_ID,Sel_Guest_ID")] WelcomeGuestVisit guestVisit)
        {
            var role = UserRole();
            if (!role.WS_CreateVisit)
            {
                return RedirectToAction("Unauthorized", "Error", new { role.Role, role.UserName});
            }

            var model = db.WelcomeGuestVisits.Find(guestVisit.GV_ID);
            if (model == null)
            {
                return HttpNotFound();
            }

            model.Guest = db.WelcomeGuests.Find(guestVisit.Sel_Guest_ID);
            if (model.Guest == null)
            {
                return HttpNotFound();
            }

            ModelState.Clear();
            TryValidateModel(model);
            if (ModelState.IsValid)
            {
                db.Entry(model).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("GuestCreate", new { id = model.Visit.Visit_ID });
            }

            guestVisit.Visit_ID = model.Visit.Visit_ID;
            //Sel_Guest_ID-t megkapta, nem kell beállítani
            guestVisit.AssignableGuests.Add(new SelectListItem()
            {
                Text = model.Guest.Fullname,
                Value = guestVisit.Sel_Guest_ID.ToString()
            });
            FillAssignableGuests(guestVisit);
            return View(guestVisit);
        }

    // GET: welcomeGuestVisits/GuestDel/5
    public ActionResult GuestDel(int? id)
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
            WelcomeGuestVisit guestVisit = db.WelcomeGuestVisits.Find(id);
            if (guestVisit == null)
            {
                return HttpNotFound();
            }
            int Visit_ID = guestVisit.Visit.Visit_ID;

            db.WelcomeGuestVisits.Remove(guestVisit);
            db.SaveChanges();
            return RedirectToAction("GuestCreate", new { id = Visit_ID });
        }
        #endregion

        #region Other functions
        /// <summary>
        /// A már elmúlt időpontú látogatásokat és a hozzá kapcsolodó vendégeket átteszi a hisztorikus adatokba.
        /// </summary>
        /// <param name="dnow"></param>
        private void OldVisitsToHist(DateTime dnow)
        {
            var oviss = db.WelcomeVisits
                .Include(v => v.GuestVisits)
                .Where(v => v.ToDT < dnow).ToList();
            if (oviss.Count() > 0)
            {
                foreach (var ov in oviss)
                {
                    WelcomeVisitHist vishist = new WelcomeVisitHist
                    {
                        Visit_ID = ov.Visit_ID,
                        Visit = ov.Visit,
                        FromDT = ov.FromDT,
                        ToDT = ov.ToDT,
                        Company = ov.Company,
                        CreatingDT = ov.CreatingDT,
                        Deleted = false
                    };
                    db.WelcomeVisitHists.Add(vishist);
                    foreach (var ogv in ov.GuestVisits)
                    {
                        if (!ogv.Guest.Deleted)
                        {
                            WelcomeGuestVisitHist gvhist = new WelcomeGuestVisitHist
                            {
                                Guest = ogv.Guest,
                                VisitHist = vishist
                            };
                            db.WelcomeGuestVisitHists.Add(gvhist);
                        }
                    }
                    db.WelcomeVisits.Remove(ov);
                }
                db.SaveChanges();
            }
        }
        /// <summary>
        /// Betölti a partnercég lenyíló adattartalmát
        /// </summary>
        /// <param name="visit">az adatmodell, ahová a lenyíló adattartalmát be kell tölteni</param>
        private void FillAssignableCompanies(WelcomeVisit visit)
        {
            //Választható cégek
            foreach (var comp in db.WelcomeCompanies.Where(x => !x.Deleted).OrderBy(c => c.Company).ToList())
            {
                visit.AssignableCompanies.Add(new SelectListItem()
                {
                    Text = comp.Company,
                    Value = comp.Comp_ID.ToString()
                });
            }
        }

        /// <summary>
        /// Betölti a vendég lenyíló adattartalmát és a már kiválasztott vendégeket
        /// </summary>
        /// <param name="guestVisit">az adatmodell, ahová a lenyíló adattartalmát be kell tölteni</param>
        private void FillAssignableGuests(WelcomeGuestVisit guestVisit)
        {
            // Már kiválasztott vendégek a view-nak megfelelően sorbarendezve
            var alrsel = guestVisit.Visit.GuestVisits
                = db.WelcomeGuestVisits
                    .Where(gv => gv.Visit.Visit_ID == guestVisit.Visit_ID)
                    .OrderByDescending(gv => gv.Guest.Rank.Importance)
                    .ThenBy(gv => gv.Guest.Fullname)
                    .ToList();
            var aslen = alrsel.Count();

            //A partnercég emberei fontossági és névsorrendben
            var compguests = guestVisit.Visit.Company.Guests
                .Where(g => !g.Deleted)
                .OrderByDescending(g => g.Rank.Importance)
                .ThenBy(g => g.Fullname);

            // A partnercég még ki nem választott embereit betölti a SelectListItem-be
            foreach (WelcomeGuest cg in compguests)
            {
                var gid = cg.Guest_ID;
                int i = 0;
                while (i < aslen && alrsel[i].Guest.Guest_ID != gid) i++;
                if (i >= aslen)
                {
                    guestVisit.AssignableGuests.Add(new SelectListItem()
                    {
                        Text = cg.Fullname,
                        Value = gid.ToString()
                    });
                }
            }
            guestVisit.hasguest = guestVisit.AssignableGuests.Count() > 0;
        }

        /// <summary>
        /// Validálja az adatmodelt időadatai alapján, ahol a kezdetnek korábban kell lenni, mint a végnek.
        /// </summary>
        /// <param name="welcomeVisit"></param> az adatmodel mely időadatait validálni kell
        private void ValidateVisitTime(WelcomeVisit welcomeVisit, DateTime now)
        {
            if (welcomeVisit.ToDT < now)
            {
                if (welcomeVisit.ToDate < now.Date)
                {
                    ModelState.AddModelError("ToDate", "Múltbeli látogatás nem rögzíthető!");
                }
                else
                {
                    ModelState.AddModelError("ToTime", "Elmúlt időpontra nem rögzíthető látogatás!");
                }
            }
            else if (welcomeVisit.FromDate > welcomeVisit.ToDate)
            {
                ModelState.AddModelError("ToDate", "A vég dátum nem lehet korábban a kezdetnél!");
            }
            else if (welcomeVisit.FromDate == welcomeVisit.ToDate && welcomeVisit.FromTime >= welcomeVisit.ToTime)
            {
                ModelState.AddModelError("ToTime", "A vég időpontnak később kell lennie a kezdetnél!");
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
