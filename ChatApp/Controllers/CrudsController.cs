using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ChatApp.Models;

namespace ChatApp.Controllers
{
    public class CrudsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Cruds
        public ActionResult Index()
        {
            return View(db.Cruds.ToList());
        }

        // GET: Cruds/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Crud crud = db.Cruds.Find(id);
            if (crud == null)
            {
                return HttpNotFound();
            }
            return View(crud);
        }

        // GET: Cruds/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Cruds/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,DateTime")] Crud crud)
        {
            if (ModelState.IsValid)
            {
                crud.DateTime = DateTime.Now;
                db.Cruds.Add(crud);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(crud);
        }

        // GET: Cruds/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Crud crud = db.Cruds.Find(id);
            if (crud == null)
            {
                return HttpNotFound();
            }
            return View(crud);
        }

        // POST: Cruds/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,DateTime")] Crud crud)
        {
            if (ModelState.IsValid)
            {
                crud.DateTime = DateTime.Now;
                db.Entry(crud).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(crud);
        }

        // GET: Cruds/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Crud crud = db.Cruds.Find(id);
            if (crud == null)
            {
                return HttpNotFound();
            }
            return View(crud);
        }

        // POST: Cruds/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Crud crud = db.Cruds.Find(id);
            db.Cruds.Remove(crud);
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
