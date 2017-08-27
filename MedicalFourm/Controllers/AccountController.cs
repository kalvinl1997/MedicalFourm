using MedicalFourm.Models;
using MedicalFourm.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
////////////delegate instialization////////
public delegate int noofquestion(int userid);
namespace MedicalFourm.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        AppDbContext Db = new AppDbContext();
        [HttpGet]
        public ActionResult LogIn(string returnUrl)
        {

            var model = new LogInModel
            {
                ReturnUrl = returnUrl
            };

            return View(model);
        }



        [HttpGet]

        public ActionResult Register()
        {

            return View();
        }

        [HttpPost]

        public ActionResult Register(Users model)
        {
            var error = ModelState.IsValid;
            if (ModelState.IsValid)
            {
                var user = Db.Users.FirstOrDefault(x => x.Email == model.Email);
                if (user == null)
                {
                    Db.Users.Add(model);
                    Db.SaveChanges();
                    var identity = new ClaimsIdentity(new[] {
                new Claim(ClaimTypes.Email, model.Email),
                new Claim(ClaimTypes.Name, model.FirstName)
            },
                   "ApplicationCookie");

                    var ctx = Request.GetOwinContext();
                    var authManager = ctx.Authentication;

                    authManager.SignIn(identity);
                    return RedirectToAction("Index", "Home", null);
                }
                else
                {
                    ModelState.AddModelError("", "User Email Already Exist Please Try Other User Name");
                    return RedirectToAction("Register", "Account", null);
                }

            }


            return View(model);
        }




        [HttpGet]

        public ActionResult EditProfile()
        {
            var Usr = Db.Users.FirstOrDefault(x => x.Email == User.Identity.Name);
            ////////Calling Delegate//////////////////////
            noofquestion delcallForquestion, delcallForAnswer;
           delcallForquestion = new noofquestion(getnoofquestion);
            ViewBag.Question = delcallForquestion(Usr.Id);
           delcallForAnswer = new noofquestion(getnoOfanswer);
            ViewBag.Answer = delcallForAnswer(Usr.Id);
            ViewBag.Readonly = true;
            return View(Usr);
        }


        [HttpPost]

        public ActionResult EditProfile(Users model)
        {

            Db.Entry(model).State = EntityState.Modified;
            Db.SaveChanges();
            return View(model);
        }




        [HttpPost]
        public ActionResult LogIn(LogInModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            // Don't do this in production!
            if (Db.Users.FirstOrDefault(x => x.Email == model.Email && x.Password == model.Password) != null)
            {
                var identity = new ClaimsIdentity(new[] {
                new Claim(ClaimTypes.Name, model.Email),
                new Claim(ClaimTypes.Email,model.Email)
            },
                    "ApplicationCookie");

                var ctx = Request.GetOwinContext();
                var authManager = ctx.Authentication;

                authManager.SignIn(identity);

                return Redirect(GetRedirectUrl(model.ReturnUrl));
            }

            // user authN failed
            ModelState.AddModelError("", "Invalid email or password");
            return View();
        }



        public ActionResult LogOut()
        {
            var ctx = Request.GetOwinContext();
            var authManager = ctx.Authentication;

            authManager.SignOut("ApplicationCookie");
            return RedirectToAction("Index", "Home");
        }


        private string GetRedirectUrl(string returnUrl)
        {
            if (string.IsNullOrEmpty(returnUrl) || !Url.IsLocalUrl(returnUrl))
            {
                return Url.Action("index", "home");
            }

            return returnUrl;
        }
        //////Function Called by the delegate/////
        public int getnoofquestion(int usrid)
        {
            int No = 0;
            var noofques = (from e in Db.Questions where e.UserId == usrid select e).Count();
            if (noofques != 0)
            {
                No = noofques;
            }
            else
            {
                No = 0;
            }
            return No;
        }
        public int getnoOfanswer(int usrid)
        {
            int Number = 0;
            var noofans = (from e in Db.Comment where e.UserId == usrid select e).Count();
            if (noofans != 0)
            {
                Number = noofans;
            }
            else
            {
                Number = 0;
            }
            return Number;
        }
    }
}