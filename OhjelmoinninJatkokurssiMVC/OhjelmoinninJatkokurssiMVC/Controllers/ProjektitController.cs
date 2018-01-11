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
        // GET: Customer
        public ActionResult Index()
        {
            ViewBag.OmaTieto = "ABC123";

            AsiakastietokantaEntities entities = new AsiakastietokantaEntities();
            List<Projektit> model = entities.Projektit.ToList();
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
            //List<Customer> model = entities.Customers.ToList();

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

        public JsonResult GetSingleProjekti(int id)
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

        public ActionResult Update(Projektit proj)
        {
            AsiakastietokantaEntities entities = new AsiakastietokantaEntities();
            string id = proj.ProjektiID.ToString();

            bool OK = false;

            // onko kyseessä muokkaus vai uuden lisääminen?
            if (id == "uusi")            {
                // kyseessä on uuden asiakkaan lisääminen, kopioidaan kentät
                Projektit dbItem = new Projektit()
                {
                //    ProjektiID = proj.Projektinimi.ToString(),                    
                };

                // tallennus tietokantaan
                entities.Projektit.Add(dbItem);
                entities.SaveChanges();
                OK = true;
            }
            else
            {
                // muokkaus, haetaan id:n perusteella riviä tietokannasta
                //Projektit dbItem = (from p in entities.Projektit
                //                   where p.ProjektiID == id
                //                   select p).FirstOrDefault();
                //if (dbItem != null)
                {
                    //dbItem.Projektinimi = proj.Projektinimi;
                   

                    // tallennus tietokantaan
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
            Projektit dbItem = (from p in entities.Projektit
                                where p.ProjektiID.ToString() == id
                               select p).FirstOrDefault();
            if (dbItem != null)
            {
                // tietokannasta poisto
                entities.Projektit.Remove(dbItem);
                entities.SaveChanges();
                OK = true;
            }
            entities.Dispose();

            return Json(OK, JsonRequestBehavior.AllowGet);
        }
    }
}