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
    public class TunnitController : Controller
    {
        // GET: Customer
        public ActionResult Index()
        {
            ViewBag.OmaTieto = "ABC123";

            AsiakastietokantaEntities entities = new AsiakastietokantaEntities();
            List<Tunnit> model = entities.Tunnit.ToList();
            entities.Dispose();

            return View(model);
        }

        public ActionResult Index2()
        {
            return View();
        }

        public ActionResult Index3()
        {
            return View();
        }

        public JsonResult GetList()
        {
            AsiakastietokantaEntities entities = new AsiakastietokantaEntities();
           // List<Tunnit> model = entities.Tunnit.ToList();

            var model = (from t in entities.Tunnit
                        select new
                         {
                             TuntiID = t.TuntiID,
                             ProjektiID = t.ProjektiID,
                             HenkiloID = t.HenkiloID,
                             Pvm = t.Pvm,
                             Projektitunnit = t.Projektitunnit
                         }).ToList();

            string json = JsonConvert.SerializeObject(model);
            entities.Dispose();

            Response.Expires = -1;
            Response.CacheControl = "no-cache";

            return Json(json, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetSingleTunti(string id)
        {
            AsiakastietokantaEntities entities = new AsiakastietokantaEntities();
            var model = (from t in entities.Tunnit
                         where t.TuntiID.ToString() == id
                         select new
                         {
                             TuntiID = t.TuntiID,
                             ProjektiID = t.ProjektiID,
                             HenkiloID = t.HenkiloID,
                             Pvm = t.Pvm,
                             Projektitunnit = t.Projektitunnit
                         }).FirstOrDefault();

            string json = JsonConvert.SerializeObject(model);
            entities.Dispose();

            return Json(json, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Update(Tunnit tunn)
        {
            AsiakastietokantaEntities entities = new AsiakastietokantaEntities();
            string id = tunn.TuntiID.ToString();

            bool OK = false;

            // onko kyseessä muokkaus vai uuden lisääminen?
            if (id == "(uusi)")
            {
                // kyseessä on uuden asiakkaan lisääminen, kopioidaan kentät
                Tunnit dbItem = new Tunnit()
                {
                    TuntiID = tunn.TuntiID,
                    ProjektiID = tunn.ProjektiID,
                    HenkiloID = tunn.HenkiloID,
                    Pvm = tunn.Pvm,
                    Projektitunnit = tunn.Projektitunnit
                };

                // tallennus tietokantaan
                entities.Tunnit.Add(dbItem);
                entities.SaveChanges();
                OK = true;
            }
            else
            {
                // muokkaus, haetaan id:n perusteella riviä tietokannasta
                Tunnit dbItem = (from t in entities.Tunnit
                                    where t.TuntiID.ToString() == id
                                    select t).FirstOrDefault();
                if (dbItem != null)
                {
                    dbItem.ProjektiID = tunn.ProjektiID;
                    dbItem.HenkiloID = tunn.HenkiloID;
                    dbItem.Pvm = tunn.Pvm;
                    dbItem.Projektitunnit = tunn.Projektitunnit;


                    //tallennus tietokantaan
                    entities.SaveChanges();
                    OK = true;
                }
            }
            entities.Dispose();

            return Json(OK, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Delete(string id)
        {
            AsiakastietokantaEntities entities = new AsiakastietokantaEntities();

            // etsitään id:n perusteella asiakasrivi kannasta
            bool OK = false;
            Tunnit dbItem = (from t in entities.Tunnit
                                where t.ProjektiID.ToString() == id
                                select t).FirstOrDefault();
            if (dbItem != null)
            {
                // tietokannasta poisto
                entities.Tunnit.Remove(dbItem);
                entities.SaveChanges();
                OK = true;
            }
            entities.Dispose();

            return Json(OK, JsonRequestBehavior.AllowGet);
        }
    }
}