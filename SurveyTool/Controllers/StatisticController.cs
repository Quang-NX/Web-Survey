using Microsoft.AspNet.Identity;
using SurveyTool.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SurveyTool.Controllers
{
    [Authorize]
    public class StatisticController : Controller
    {
        private readonly ApplicationDbContext _db;

        public StatisticController(ApplicationDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();
            var responses = _db.Responses.Where(x => x.Survey.UserId.Equals(userId))
                             .Include("Survey")
                             .Include("Answers")
                             .Include("Answers.Question")
                             .OrderByDescending(x => x.CreatedOn)
                             .ThenByDescending(x => x.Id)
                             .ToList();
            return View(responses);
        }
        
        //Get response folow month
        public ActionResult GetResponseToMonth()
        {
            var userId = User.Identity.GetUserId();
            var year = DateTime.Now.Year;
            var query = _db.Responses
                .Where(x => x.CreatedOn.Year == year && x.Survey.UserId.Equals(userId))
                .GroupBy(x => x.CreatedOn.Month)
                .Select(x => new { name = x.Key, count = x.Count() })
                .ToList();
            return Json(query, JsonRequestBehavior.AllowGet);
        }
        //Get persen question type
        public ActionResult GetPersenQuestionType()
        {
            var userId = User.Identity.GetUserId();
            //get all survey of user
            var surveys = _db.Surveys
                .Where(x => x.UserId.Equals(userId))
                .Include(x => x.Questions)
                .ToList();
            int email = 0;
            int yesno = 0;
            int correct = 0;
            int text = 0;
            int number = 0;
            int ques = 0;

            foreach (var survey in surveys)
            {
                foreach (var question in survey.Questions)
                {
                    if (question.Type.Equals("Email"))
                    {
                        email++;
                    }
                    else if (question.Type.Equals("Yes/No"))
                    {
                        yesno++;
                    }
                    else if (question.Type.Equals("Number"))
                    {
                        number++;
                    }
                    else if (question.Type.Equals("Correct/Incorrect"))
                    {
                        correct++;
                    }
                    else
                    {
                        text++;
                    }
                    ques++;
                }

            }
            List<Chart> data = new List<Chart>();
            data.Add(new Chart("Email",CalculateScore(email,ques)));
            data.Add(new Chart("Có/Không", CalculateScore(yesno, ques)));
            data.Add(new Chart("Số", CalculateScore(number, ques)));
            data.Add(new Chart("Đúng/Sai", CalculateScore(correct, ques)));
            data.Add(new Chart("Chữ", CalculateScore(text, ques)));

            return Json(data, JsonRequestBehavior.AllowGet);

        }
        public ActionResult Delete(int surveyId, int id, string returnTo)
        {
            var response = _db.Responses.Where(x => x.SurveyId == surveyId && x.Id == id).Single();
            _db.Responses.Remove(response);
            _db.Entry(response).State = EntityState.Deleted;
            _db.SaveChanges();
            return Redirect(returnTo ?? Url.RouteUrl("Statistic"));
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
                              .Single(x => x.Id == id);
            response.Answers = response.Answers.OrderBy(x => x.Question.Priority).ToList();
            return View(response);
        }
        public double CalculateScore(int answers, int totalQuestion)
        {
            if (totalQuestion == 0 || answers == 0)
                return 100*0.0;

            return 100*((double)answers / (double)totalQuestion);
        }
    }

}