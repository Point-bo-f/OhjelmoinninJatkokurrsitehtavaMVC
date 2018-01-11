using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using OhjelmoinninJatkokurssiMVC.Models;

namespace AsiakastietokantaMVC.Controllers
{
    public class HenkilotController : Controller
    {
        AsiakastietokantaEntities db = new AsiakastietokantaEntities();

        // GET: Henkilots
        public ActionResult Index()
        {
            {
                List<Henkilot> model = new List<Henkilot>();
                try
                {
                    AsiakastietokantaEntities entities = new AsiakastietokantaEntities();
                    model = entities.Henkilot.ToList();

                    entities.Dispose();
                }

                catch (Exception ex)
                {
                    ViewBag.ErrorMessage = ex.GetType() + ": " + ex.Message;
                }

                return View(model);
            }
        }

        public ActionResult Index2()
        {
            return View();
        }

        // GET: Henkilots/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Henkilot henkilot = db.Henkilot.Find(id);
            if (henkilot == null)
            {
                return HttpNotFound();
            }
            return View(henkilot);
        }

        // GET: Henkilots/Create
        public ActionResult Create()
        {
            AsiakastietokantaEntities db = new AsiakastietokantaEntities();

            Henkilot model = new Henkilot();

            return View(model);


        }

        // POST: Henkilots/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Henkilot model)
        {
            AsiakastietokantaEntities db = new AsiakastietokantaEntities();

            Henkilot henkilot = new Henkilot();
            henkilot.HenkiloID = model.HenkiloID;
            henkilot.Etunimi = model.Etunimi;
            henkilot.Sukunimi = model.Sukunimi;
            henkilot.Osoite = model.Osoite;
            henkilot.Esimies = model.Esimies;

            db.Henkilot.Add(henkilot);

            try
            {
                db.SaveChanges();
            }

            catch (Exception ex)
            {
            }


            return RedirectToAction("Index");
        }




        // GET: Henkilots/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Henkilot henkilot = db.Henkilot.Find(id);
            if (henkilot == null)
            {
                return HttpNotFound();
            }
            return View(henkilot);
        }

        // POST: Henkilots/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "HenkiloID,Etunimi,Sukunimi,Osoite,Esimies")] Henkilot henkilot)
        {
            if (ModelState.IsValid)
            {
                db.Entry(henkilot).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(henkilot);
        }

        // GET: Henkilots/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Henkilot henkilot = db.Henkilot.Find(id);
            if (henkilot == null)
            {
                return HttpNotFound();
            }
            return View(henkilot);
        }

        // POST: Henkilots/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Henkilot henkilot = db.Henkilot.Find(id);
            db.Henkilot.Remove(henkilot);
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

        public JsonResult GetList()
        {
            AsiakastietokantaEntities entities = new AsiakastietokantaEntities();

            var model = (from h in entities.Henkilot
                         select new
                         {
                             HenkiloID = h.HenkiloID,
                             Etunimi = h.Etunimi,
                             Sukunimi = h.Sukunimi,
                             Osoite = h.Osoite,
                             Esimies = h.Esimies
                         }).ToList();

            string json = JsonConvert.SerializeObject(model);
            entities.Dispose();

            Response.Expires = -1;
            Response.CacheControl = "no-cache";

            return Json(json, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetSingleHenkilo(int id)
        {
            AsiakastietokantaEntities entities = new AsiakastietokantaEntities();

            var model = (from h in entities.Henkilot
                         where h.HenkiloID == id
                         select new
                         {
                             HenkiloID = h.HenkiloID,
                             Etunimi = h.Etunimi,
                             Sukunimi = h.Sukunimi,
                             Osoite = h.Osoite,
                             Esimies = h.Esimies
                         }).FirstOrDefault();

            string json = JsonConvert.SerializeObject(model);
            entities.Dispose();

            return Json(json, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Edit2(Henkilot henk)
        {
            AsiakastietokantaEntities entities = new AsiakastietokantaEntities();

            bool OK = false;

            if (henk.HenkiloID == 0)
            {
                Henkilot dbItem = new Henkilot()
                {
                    Etunimi = henk.Etunimi,
                    Sukunimi = henk.Sukunimi,
                    Osoite = henk.Osoite,
                    Esimies = henk.Esimies
                };

                entities.Henkilot.Add(dbItem);
                entities.SaveChanges();
                OK = true;
            }
            else
            {
                Henkilot dbItem = (from h in entities.Henkilot
                                   where h.HenkiloID == henk.HenkiloID
                                   select h).FirstOrDefault();


                if (dbItem != null)
                {
                    dbItem.HenkiloID = henk.HenkiloID;
                    dbItem.Etunimi = henk.Etunimi;
                    dbItem.Sukunimi = henk.Sukunimi;
                    dbItem.Osoite = henk.Osoite;
                    dbItem.Esimies = henk.Esimies;


                    entities.SaveChanges();
                    OK = true;
                }
            }
            entities.Dispose();

            return Json(OK, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Delete(int id)
        {
            AsiakastietokantaEntities entities = new AsiakastietokantaEntities();

            bool OK = false;
            Henkilot dbItem = (from h in entities.Henkilot
                               where h.HenkiloID == id
                               select h).FirstOrDefault();
            if (dbItem != null)
            {
                entities.Henkilot.Remove(dbItem);
                entities.SaveChanges();
                OK = true;
            }
            entities.Dispose();

            return Json(OK, JsonRequestBehavior.AllowGet);
        }
    }
}
