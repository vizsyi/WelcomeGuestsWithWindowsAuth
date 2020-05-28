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
    public class IntraRolesController : IntraController
    {
        //private IntranetContext db = new IntranetContext();

        // GET: IntraRoles
        public ActionResult Index()
        {
            var role = UserRole();
            if (!role.Admin)
            {
                return RedirectToAction("Unauthorized", "Error", new { role.Role, role.UserName});
            }

            return View(db.IntraRoles
                .OrderByDescending(r => r.Admin)
                .ThenBy(r => r.Role)
                .ToList());
        }

        // GET: IntraRoles/Details/5
        public ActionResult Details(int? id)
        {
            var role = UserRole();
            if (!role.Admin)
            {
                return RedirectToAction("Unauthorized", "Error", new { role.Role, role.UserName});
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            IntraRole intraRole = db.IntraRoles.Find(id);
            if (intraRole == null)
            {
                return HttpNotFound();
            }
            return View(intraRole);
        }

        // GET: IntraRoles/Create
        public ActionResult Create()
        {
            var role = UserRole();
            if (!role.Admin)
            {
                return RedirectToAction("Unauthorized", "Error", new { role.Role, role.UserName});
            }

            return View();
        }

        // POST: IntraRoles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Role,Admin,UsefullLimks,WS_ViewVisit,WS_CreateVisit,WS_DelCompany,WS_Settings,T2rT_RecordChecklist,T2rT_EditChecklist")] IntraRole intraRole)
        {
            var role = UserRole();
            if (!role.Admin)
            {
                return RedirectToAction("Unauthorized", "Error", new { role.Role, role.UserName});
            }

            if (ModelState.IsValid)
            {
                db.IntraRoles.Add(intraRole);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(intraRole);
        }

        // GET: IntraRoles/Edit/5
        public ActionResult Edit(int? id)
        {
            var role = UserRole();
            if (!role.Admin)
            {
                return RedirectToAction("Unauthorized", "Error", new { role.Role, role.UserName});
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            IntraRole intraRole = db.IntraRoles.Find(id);
            if (intraRole == null)
            {
                return HttpNotFound();
            }
            return View(intraRole);
        }

        // POST: IntraRoles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Role_ID,Role,Admin,UsefullLimks,WS_ViewVisit,WS_CreateVisit,WS_DelCompany,WS_Settings,T2rT_RecordChecklist,T2rT_EditChecklist")] IntraRole intraRole)
        {
            var role = UserRole();
            if (!role.Admin)
            {
                return RedirectToAction("Unauthorized", "Error", new { role.Role, role.UserName});
            }

            //itt kellene kikeresni (find(id)) a megfelelő kategóriákat
            //category = db.Categories.Find(id);

            //ezzel bemutatom az intraRole-t az EntityFramework-nek
            //innen fogja tudni majd betölteni a navigációs property-t is
            //db.IntraRoles.Attach(intraRole);//

            //ezzel az adatok mentését készítem elő
            //var roleEntry = db.Entry(intraRole);

            //betölti a navigációs property-t (ha nem kommentelném ki)
            //ezzel egyidőben az EntityFramework tudomást szerez a létezéséről és a változást már el is menti
            //roleEntry.Reference(x => x.Category)
            //    .Load();

            //beállítja a megfelelő kategóriát
            //intraRole.Category = category

            //ModelState.Clear();
            //var isValid = TryUpdateModel(intraRole);

            var model = db.IntraRoles.Find(intraRole.Role_ID);
            if (model == null)
            {
                return HttpNotFound();
            }

            model.Assign(intraRole);

            ModelState.Clear();
            TryUpdateModel(model);
            if (ModelState.IsValid)
            {
                //ez jelzi az EF-nek, hogy módosult az intraRole
                //roleEntry.State = EntityState.Modified;

                db.Entry(model).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }

        // GET: IntraRoles/Delete/5
        public ActionResult Delete(int? id)
        {
            var role = UserRole();
            if (!role.Admin)
            {
                return RedirectToAction("Unauthorized", "Error", new { role.Role, role.UserName});
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            IntraRole intraRole = db.IntraRoles.Find(id);
            if (intraRole == null)
            {
                return HttpNotFound();
            }
            return View(intraRole);
        }

        // POST: IntraRoles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var role = UserRole();
            if (!role.Admin)
            {
                return RedirectToAction("Unauthorized", "Error", new { role.Role, role.UserName});
            }

            IntraRole intraRole = db.IntraRoles.Find(id);
            db.IntraRoles.Remove(intraRole);
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
