using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MedicalFourm.Models;
using MedicalFourm.Models.ViewModels;

namespace MedicalFourm.Controllers
{
    [Authorize]
    public class QuestionsController : Controller
    {
        private AppDbContext db = new AppDbContext();
        public event Clandervisibiltyeventhandler calendervsisbltchng;
        // GET: Questions
        public ActionResult ShowAnswer()
        {
            var answers = db.Comment.Where(x => x.Usr.Email == User.Identity.Name);
            return View(answers.ToList());
        }

        public ActionResult Index()
        {
            var questions = db.Questions.Where(x=>x.Usr.Email==User.Identity.Name);
            return View(questions.ToList());
        }

        [AllowAnonymous]
        // GET: Questions/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = db.Users.FirstOrDefault(x => x.Email == User.Identity.Name);
            Question question = db.Questions.Find(id);
            var comments = db.Comment.Where(x => x.QuestionId == id).ToList();
            int dislikes = db.Choices.Where(x => x.QuestionId == id && x.IsDisliked == true).ToList().Count();
            int likes = db.Choices.Where(x => x.QuestionId == id && x.IsLiked == true).ToList().Count();
            bool isuerdislike = false;
            bool isuerlike = false;
            try {
              isuerlike = Convert.ToBoolean(db.Choices.FirstOrDefault(x => x.UserId == user.Id && x.QuestionId == id).IsLiked);

               isuerdislike = Convert.ToBoolean(db.Choices.FirstOrDefault(x => x.UserId == user.Id && x.QuestionId == id).IsDisliked);



            }
            catch (Exception ex)
            {

            }


            QuestionVM questionvm = new QuestionVM()
            {
                username = question.Usr.username,
                Question = question,
                Comments = comments,
                 dislikes=dislikes,
                 likes=likes,
                 isuserdislike=isuerdislike,
                  isuserlike=isuerlike

            };
            if (question == null)
            {
                return HttpNotFound();
            }
            return View(questionvm);
        }



        [AllowAnonymous]
        public ActionResult AddLike(int id)
        {
            if (!Request.IsAuthenticated)
            {
                return RedirectToAction("LogIn", "Account", new { returnUrl = "Questions/Details/" + id });
            }
            var user = db.Users.FirstOrDefault(x => x.Email == User.Identity.Name);
            Choice choice = new Choice()
            {
                IsDisliked = false,
                IsLiked = true,
                QuestionId = id,
                UserId = user.Id
            };

            var choicee = db.Choices.FirstOrDefault(x => x.UserId == user.Id && x.QuestionId == id);
           if (choicee==null){

                db.Choices.Add(choice);
               
            }
            else
            {
                choicee.IsDisliked = false;
                choicee.IsLiked = true;
                db.Entry(choicee).State = EntityState.Modified;
               
            }
            db.SaveChanges();
            return RedirectToAction("Details", new { id });
        }

        [AllowAnonymous]
        public ActionResult AddDislike(int id)
        {
            if (!Request.IsAuthenticated)
            {
                return RedirectToAction("LogIn", "Account", new { returnUrl = "Questions/Details/"+id });
            }


            var user = db.Users.FirstOrDefault(x => x.Email == User.Identity.Name);
            Choice choice = new Choice()
            {
                IsDisliked = true,
                IsLiked = false,
                QuestionId = id,
                UserId = user.Id
            };

            var choicee = db.Choices.FirstOrDefault(x => x.UserId == user.Id && x.QuestionId == id);
            if (choicee == null)
            {

                db.Choices.Add(choice);

            }
            else
            {
                choicee.IsDisliked = true;
                choicee.IsLiked = false;
                db.Entry(choicee).State = EntityState.Modified;

            }
            db.SaveChanges();

            return RedirectToAction("Details", new { id });

        }

// GET: Questions/Create
public ActionResult Create()
        {
            return View();
        }

        // POST: Questions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]

        public ActionResult Create(Question question)
        {
            if (ModelState.IsValid)
            {
                question.PublishedOn = DateTime.Now;
                question.LastUpdated = DateTime.Now;
                question.UserId = db.Users.FirstOrDefault(x => x.Email == User.Identity.Name).Id;

                db.Questions.Add(question);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            // ViewBag.UserId = new SelectList(db.Users, "Id", "Name", question.UserId);
            return View(question);
        }
        [AllowAnonymous]
        [HttpPost]
        public ActionResult AddComment(string Commenttxt, int id)
        {
            if (Request.IsAuthenticated) {
                var user = db.Users.FirstOrDefault(x => x.Email == User.Identity.Name);
                db.Comment.Add(new Comments()
                {
                    CommentText = Commenttxt,
                    QuestionId = id,
                    CommetedOn = DateTime.Now,
                    LastUpdatedOn = DateTime.Now,
                    UserId = user.Id
               });
            db.SaveChanges();
            var comments = db.Comment.Where(x => x.QuestionId == id).ToList();
            return PartialView(comments);

        }
            else
            {
                ViewBag.returnURL = "Questions/"+id;
                return PartialView("NOTAuthenticated");
    }
}


        [AllowAnonymous]
        public string removeLike(int id) {


            return "";
        }





        // GET: Questions/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Question question = db.Questions.Find(id);
            if (question == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserId = new SelectList(db.Users, "Id", "Name", question.UserId);
            return View(question);
        }

        // POST: Questions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Qstatment,PublishedOn,LastUpdated,UserId")] Question question)
        {
            if (ModelState.IsValid)
            {
                question.LastUpdated = DateTime.Now;
                db.Entry(question).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UserId = new SelectList(db.Users, "Id", "Name", question.UserId);
            return View(question);
        }

        // GET: Questions/Delete/5
        [AllowAnonymous]
        public ActionResult DeleteComments(int? id,int qid)
        {
  
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comments comment = db.Comment.Find(id);
            if (comment == null)
            {
                return HttpNotFound();
            }

            db.Comment.Remove(comment);
            db.SaveChanges();
            var comments = db.Comment.Where(x => x.QuestionId == qid).ToList();
            return PartialView("AddComment", comments);
       
        }

        // POST: Questions/Delete/5
        [AllowAnonymous]

 
        public ActionResult Delete(int id)
        {
            Question question = db.Questions.Find(id);
            var com = db.Comment.Where(x => x.QuestionId == id).ToList();
            if (com != null)
            {
                foreach (var item in com)
                {
                    db.Comment.Remove(item);
                }
               
            }
            var likesordislikes = db.Choices.Where(x => x.QuestionId == id);
            if (likesordislikes != null)
            {
                foreach (var i in likesordislikes)
                {
                    db.Choices.Remove(i);
                }

            }
            db.Questions.Remove(question);
            db.SaveChanges();
            return PartialView(db.Questions.Where(x => x.Usr.Email == User.Identity.Name).ToList());
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

    public class clandervisibiltyeventArg:EventArgs
    {
        private bool _isclandervisible;

        public bool Iscalendervisble
        {
            get { return this._isclandervisible; }

        }
        public clandervisibiltyeventArg(bool iscalandervisble)
        {
            this._isclandervisible = iscalandervisble;
        }
        

    }
    public delegate void Clandervisibiltyeventhandler(object sender, clandervisibiltyeventArg e);
}