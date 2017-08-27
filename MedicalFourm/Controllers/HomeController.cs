using MedicalFourm.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MedicalFourm.Controllers
{
    public class HomeController : Controller
    {
        AppDbContext db = new AppDbContext();
   

        public ActionResult Index(string search)
        {
            //var countdis = from e in db.Questions
            //               join f in db.Choices
            //               on e.Id equals f
            if (search != "" && search != null)
            {

                var quest1 = db.Questions.Where(x => x.Qcatagory.Contains(search)).ToList();
               
                if (quest1 != null)
                {
                    return View(quest1);
                }
                else
                {
                    var quest = db.Questions.Where(x => x.Qstatment.Contains(search)).ToList();
                    return View(quest);
                }
            }
            
                return View(db.Questions.ToList());
          
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}