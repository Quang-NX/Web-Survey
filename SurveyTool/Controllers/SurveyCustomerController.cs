using SurveyTool.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace SurveyTool.Controllers
{
    public class SurveyCustomerController : Controller
    {
        private readonly ApplicationDbContext _db;
        public SurveyCustomerController(ApplicationDbContext db)
        {
            this._db = db;
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
            
            return View(survey);
        }

        [HttpPost]
        public ActionResult CreateResponse(int surveyId, string action, Response model)
        {
            if (Session["customer"] == null)
            {
                CustomerSession customerSession = new CustomerSession();
                customerSession.NameIp = GetIp();
                customerSession.TotalSurvey = 1;
                customerSession.TotalSurvey -= 1;
                Session["customer"] = customerSession;
            }
            else
            {
                CustomerSession cus = (CustomerSession)Session["customer"];
                if (cus.NameIp == GetIp() && cus.TotalSurvey <= 0)
                {
                    return RedirectToAction("NotSubmitSurvey");
                }
                cus.TotalSurvey -= 1;
                Session["customer"] = cus;
            }
            bool checkInvalidEmail = false;

            model.Answers = model.Answers.Where(a => !String.IsNullOrEmpty(a.Value)).ToList();
            model.SurveyId = surveyId;
            model.CreatedOn = DateTime.Now;
            //Save infomation customer response
            var customer = new Customer();
            foreach (var item in model.Answers)
            {
                var question = _db.Questions.Where(x => x.Id == item.QuestionId).FirstOrDefault();
                if (question.Type.Equals("Email"))
                {
                    customer.Email = item.Value;
                    customer.Name = item.Value;
                    model.CreatedBy = item.Value;
                    checkInvalidEmail = true;
                }
            }
            if(checkInvalidEmail==false)
            {
                model.CreatedBy = "Khách vãng lai";

            }
            _db.Customers.Add(customer);
            _db.SaveChanges();

            model.CusId = customer.Id;
            _db.Responses.Add(model);
            _db.SaveChanges();
            return RedirectToAction("PageResponse",new {surveyId});
        }
        public ActionResult PageResponse(int surveyId)
        {
            ViewBag.SurveyId = surveyId;
            return View();
        }
        public ActionResult NotSubmitSurvey()
        {
            return View();
        }
        public string GetIp()
        {
            IPHostEntry host;
            string localIP = "?";
            host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily.ToString() == "InterNetwork")
                {
                    localIP = ip.ToString();
                }
            }
            return localIP;
        }
    }
}