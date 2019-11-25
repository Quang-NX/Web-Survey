using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using Microsoft.AspNet.Identity;
using SurveyTool.Models;
using SurveyTool.Models.ViewModel;

namespace SurveyTool.Controllers
{
    [Authorize]
    public class SurveysController : Controller
    {
        private readonly ApplicationDbContext _db;

        public SurveysController(ApplicationDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();
            var surveys = _db.Surveys.Where(x => x.UserId.Equals(userId)).ToList();
            return View(surveys);
        }

        [HttpGet]
        public ActionResult Create()
        {
            var survey = new Survey
            {
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddYears(1)
            };
            var userId = User.Identity.GetUserId();
            survey.UserId = userId;
            return View(survey);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(Survey survey, string action)
        {
            int i = 1;
            var userId = User.Identity.GetUserId();
            survey.UserId = userId;
            //create question email if required email
            if(survey.RequiredEmail)
            {
                var ques = new Question();
                ques.IsActive = true;
                ques.Required = true;
                ques.Type = "Email";
                ques.Priority = 0;
                ques.Title = "Thu thập email người dùng";
                ques.Body = "Nhập email của bạn";
                ques.SurveyId = survey.Id;
                survey.Questions.Add(ques);
                foreach (var item in survey.Questions)
                {
                    if(!item.Type.Equals("Email"))
                    {
                        item.Priority = i;
                        i++;
                    }
                    else
                    {
                        continue;
                    }
                }
            }

            //var errors = ModelState
            //    .Where(x => x.Value.Errors.Count > 0)
            //    .Select(x => new { x.Key, x.Value.Errors })
            //    .ToArray();

            //cập nhật thời gian tạo và thay đổi = thời gian hiện tại
            survey.Questions.ForEach(q => q.CreatedOn = q.ModifiedOn = DateTime.Now);

            _db.Surveys.Add(survey);
            _db.SaveChanges();
            TempData["success"] = "Khảo sát đã được tạo thành công!";
            return RedirectToAction("Edit", new { id = survey.Id });
            //}
            //else
            //{
            //    TempData["error"] = "Đã có lỗi xảy ra trong quá trình tạo";
            //    return View(survey);
            //}
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var survey = _db.Surveys.Include("Questions").Single(x => x.Id == id);
            survey.Questions = survey.Questions.OrderBy(q => q.Priority).ToList();
            var userId = User.Identity.GetUserId();
            survey.UserId = userId;
            return View(survey);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(Survey model)
        {
            var userId = User.Identity.GetUserId();
            model.UserId = userId;
            foreach (var question in model.Questions)
            {
                question.SurveyId = model.Id;

                if(question.Type.Equals("Email"))
                {
                    if(model.RequiredEmail)
                    {
                        question.Required = true;
                    }
                    else
                    {
                        question.Required = false;
                    }
                }

                if (question.Id == 0)
                {
                    question.CreatedOn = DateTime.Now;
                    question.ModifiedOn = DateTime.Now;
                    _db.Entry(question).State = EntityState.Added;
                }
                else
                {
                    question.ModifiedOn = DateTime.Now;
                    _db.Entry(question).State = EntityState.Modified;
                    _db.Entry(question).Property(x => x.CreatedOn).IsModified = false;
                }
            }

            _db.Entry(model).State = EntityState.Modified;
            _db.SaveChanges();
            return RedirectToAction("Edit", new { id = model.Id });
        }

        [HttpPost]
        public ActionResult Delete(Survey survey)
        {
            _db.Entry(survey).State = EntityState.Deleted;
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
