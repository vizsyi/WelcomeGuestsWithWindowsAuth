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
    public class WelcomeCompaniesController : IntraController
    {
        //private IntranetContext db = new IntranetContext();

        // GET: WelcomeCompanies
        public ActionResult Index()
        {
            var role = UserRole();
            if (!role.WS_CreateVisit)
            {
                return RedirectToAction("Unauthorized", "Error", new { role.Role, role.UserName});
            }

            return View(db.WelcomeCompanies
                            .Where(c => !c.Deleted)
                            .OrderBy(c => c.Company)
                            .ToList());
        }

        // GET: WelcomeCompanies/Details/5
        public ActionResult Details(int? id)
        {
            var role = UserRole();
            if (!role.WS_Settings)
            {
                return RedirectToAction("Unauthorized", "Error", new { role.Role, role.UserName });
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            WelcomeCompany welcomeCompany = db.WelcomeCompanies.Find(id);
            if (welcomeCompany == null)
            {
                return HttpNotFound();
            }
            return View(welcomeCompany);
        }

        // GET: WelcomeCompanies/Create
        public ActionResult Create()
        {
            var role = UserRole();
            if (!role.WS_CreateVisit)
            {
                return RedirectToAction("Unauthorized", "Error", new { role.Role, role.UserName});
            }

            return View();
        }

        // POST: WelcomeCompanies/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Company")] WelcomeCompany welcomeCompany)
        {
            var role = UserRole();
            if (!role.WS_CreateVisit)
            {
                return RedirectToAction("Unauthorized", "Error", new { role.Role, role.UserName});
            }

            if (ModelState.IsValid)
            {
                welcomeCompany.ShowComp = true;
                db.WelcomeCompanies.Add(welcomeCompany);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(welcomeCompany);
        }

        // GET: WelcomeCompanies/Edit/5
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
            WelcomeCompany welcomeCompany = db.WelcomeCompanies.Find(id);
            if (welcomeCompany == null)
            {
                return HttpNotFound();
            }
            return View(welcomeCompany);
        }

        // POST: WelcomeCompanies/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Comp_ID,Company,ShowComp")] WelcomeCompany welcomeCompany)
        {
            var role = UserRole();
            if (!role.WS_CreateVisit)
            {
                return RedirectToAction("Unauthorized", "Error", new { role.Role, role.UserName});
            }

            if (ModelState.IsValid)
            {
                db.Entry(welcomeCompany).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(welcomeCompany);
        }

        // GET: WelcomeCompanies/Delete/5
        public ActionResult Delete(int? id)
        {
            var role = UserRole();
            if (!role.WS_DelCompany)
            {
                return RedirectToAction("Unauthorized", "Error", new { role.Role, role.UserName});
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            WelcomeCompany welcomeCompany = db.WelcomeCompanies.Find(id);
            if (welcomeCompany == null)
            {
                return HttpNotFound();
            }
            return View(welcomeCompany);
        }

        // POST: WelcomeCompanies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var role = UserRole();
            if (!role.WS_DelCompany)
            {
                return RedirectToAction("Unauthorized", "Error", new { role.Role, role.UserName});
            }

            WelcomeCompany welcomeCompany = db.WelcomeCompanies.Find(id);
            if (welcomeCompany == null)
            {
                return HttpNotFound();
            }

            int teszt = 0;
            if (welcomeCompany.Guests.Count() > 0
                || db.WelcomeVisits.Where(v => v.Company.Comp_ID == id).Count() > 0
                || db.WelcomeVisitHists.Where(vh => vh.Company.Comp_ID == id).Count() > 0)
            {
                welcomeCompany.Deleted = true;
                db.Entry(welcomeCompany).State = EntityState.Modified;
                teszt = 1;
            }
            else
            {
                db.WelcomeCompanies.Remove(welcomeCompany);
                teszt = 2;
            }
            db.SaveChanges();

            return RedirectToAction("Index");
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
