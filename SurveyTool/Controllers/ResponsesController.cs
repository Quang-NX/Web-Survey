using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using SurveyTool.Models;

namespace SurveyTool.Controllers
{
    [Authorize]
    public class ResponsesController : Controller
    {
        private readonly ApplicationDbContext _db;

        public ResponsesController(ApplicationDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public ActionResult Index(int surveyId)
        {
            var userId = User.Identity.GetUserId();
            var responses = _db.Responses
                               .Include("Survey")
                               .Include("Answers")
                               .Include("Answers.Question")
                               .Where(x=>x.Survey.UserId.Equals(userId))
                               .Where(x => x.SurveyId == surveyId)
                               .Where(x => x.CreatedBy == User.Identity.Name)
                               .OrderByDescending(x => x.CreatedOn)
                               .ThenByDescending(x => x.Id)
                               .ToList();
            if (responses == null)
            {
                return Redirect("/pages/404");
            }
            return View(responses);
        }
        //Hiển thị lịch sử những lần khảo sát
        [HttpGet]
        public ActionResult Details(int surveyId, int id)
        {
            var response = _db.Responses
                              .Include("Survey")
                              .Include("Answers")
                              .Include("Answers.Question")
                              .Where(x => x.SurveyId == surveyId)
                              .Where(x => x.CreatedBy == User.Identity.Name)
                              .Single(x => x.Id == id);

            response.Answers = response.Answers.OrderBy(x => x.Question.Priority).ToList();
            if (response == null)
            {
                return Redirect("/pages/404");
            }
            return View(response);
        }

        [HttpGet]
        public ActionResult Create(int surveyId)
        {
            var survey = _db.Surveys
                            .Where(s => s.Id == surveyId)
                            .Select(s => new
                                {
                                    Survey = s,
                                    Questions = s.Questions
                                                 .Where(q => q.IsActive)
                                                 .OrderBy(q => q.Priority)
                                })
                             .AsEnumerable()
                             .Select(x =>
                                 {
                                     x.Survey.Questions = x.Questions.ToList();
                                     return x.Survey;
                                 })
                             .Single();
            if(survey==null)
            {
                return Redirect("/pages/404");
            }
            return View(survey);
        }

        [HttpPost]
        public ActionResult Create(int surveyId, string action, Response model)
        {
            model.Answers = model.Answers.Where(a => !String.IsNullOrEmpty(a.Value)).ToList();
            model.SurveyId = surveyId;
            model.CreatedBy = User.Identity.Name;
            model.CreatedOn = DateTime.Now;
            //Save infomation customer response
            var customer = new Customer();
            foreach(var item in model.Answers)
            {
                var question = _db.Questions.Where(x => x.Id == item.QuestionId).FirstOrDefault();
                if (question.Type.Equals("Email"))
                {
                    customer.Email = item.Value;
                    customer.Name = item.Value;
                }
            }
            _db.Customers.Add(customer);
            _db.SaveChanges();
            model.CusId = customer.Id;
            _db.Responses.Add(model);
            _db.SaveChanges();

            TempData["success"] = "Bài khảo sát đã được lưu!";

            return action == "Next"
                       ? RedirectToAction("Create", new {surveyId})
                       : RedirectToAction("Index", "Home");
        }

        public ActionResult Delete(int surveyId, int id, string returnTo)
        {
            var response = new Response() { Id = id, SurveyId = surveyId };
            _db.Entry(response).State = EntityState.Deleted;
            _db.SaveChanges();
            return Redirect(returnTo ?? Url.RouteUrl("Root"));
        }
    }
}