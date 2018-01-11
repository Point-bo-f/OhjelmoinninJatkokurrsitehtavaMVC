using Newtonsoft.Json;
using OhjelmoinninJatkokurssiMVC.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace OhjelmoinninJatkokurssiMVC.Controllers
{
    public class ProjektitController : Controller
    {
        AsiakastietokantaEntities db = new AsiakastietokantaEntities();

        // GET: Henkilots
        public ActionResult Index()
        {
            {
                List<Projektit> model = new List<Projektit>();
                try
                {
                    AsiakastietokantaEntities entities = new AsiakastietokantaEntities();
                    model = entities.Projektit.ToList();

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
            Projektit projektit = db.Projektit.Find(id);
            if (projektit == null)
            {
                return HttpNotFound();
            }
            return View(projektit);
        }

        // GET: Henkilots/Create
        public ActionResult Create()
        {
            AsiakastietokantaEntities db = new AsiakastietokantaEntities();

            Projektit model = new Projektit();

            return View(model);


        }

        // POST: Henkilots/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Projektit model)
        {
            AsiakastietokantaEntities db = new AsiakastietokantaEntities();

            Projektit projektit = new Projektit();
            projektit.ProjektiID = model.ProjektiID;
            projektit.Projektinimi = model.Projektinimi;
           
            db.Projektit.Add(projektit);

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
            Projektit projektit = db.Projektit.Find(id);
            if (projektit == null)
            {
                return HttpNotFound();
            }
            return View(projektit);
        }

        // POST: Henkilots/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProjektiID,Projektinimi")] Projektit projektit)
        {
            if (ModelState.IsValid)
            {
                db.Entry(projektit).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(projektit);
        }

        // GET: Henkilots/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
           Projektit projektit = db.Projektit.Find(id);
            if (projektit == null)
            {
                return HttpNotFound();
            }
            return View(projektit);
        }

        // POST: Henkilots/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Projektit projektit = db.Projektit.Find(id);
            db.Projektit.Remove(projektit);
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

            var model = (from p in entities.Projektit
                         select new
                         {
                             ProjektiID = p.ProjektiID,
                             Projektinimi = p.Projektinimi,
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

            var model = (from p in entities.Projektit
                         where p.ProjektiID == id
                         select new
                         {
                             ProjektiID = p.ProjektiID,
                             Projektinimi = p.Projektinimi,
                         }).FirstOrDefault();

            string json = JsonConvert.SerializeObject(model);
            entities.Dispose();

            return Json(json, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Edit2(Projektit proj)
        {
            AsiakastietokantaEntities entities = new AsiakastietokantaEntities();

            bool OK = false;

            if (proj.ProjektiID == 0)
            {
                Projektit dbItem = new Projektit()
                {
                    ProjektiID = proj.ProjektiID,
                    Projektinimi = proj.Projektinimi,
                };

                entities.Projektit.Add(dbItem);
                entities.SaveChanges();
                OK = true;
            }
            else
            {
                Projektit dbItem = (from p in entities.Projektit
                                   where p.ProjektiID == proj.ProjektiID
                                   select p).FirstOrDefault();


                if (dbItem != null)
                {
                    dbItem.ProjektiID = proj.ProjektiID;
                    dbItem.Projektinimi = proj.Projektinimi;
                    
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
            Projektit dbItem = (from p in entities.Projektit
                               where p.ProjektiID == id
                               select p).FirstOrDefault();
            if (dbItem != null)
            {
                entities.Projektit.Remove(dbItem);
                entities.SaveChanges();
                OK = true;
            }
            entities.Dispose();

            return Json(OK, JsonRequestBehavior.AllowGet);
        }
    }
}
