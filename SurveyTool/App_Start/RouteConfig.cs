using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace SurveyTool
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Statistic",
                url: "Statistic",
                defaults: new { controller = "Statistic", action = "Index" });

            routes.MapRoute(
                name: "Root",
                url: "",
                defaults: new { controller = "Dashboard", action = "Index" });

            routes.MapRoute(
                name: "Responses",
                url: "Surveys/{surveyId}/Responses/{action}/{id}",
                defaults: new { controller = "Responses", action = "Index", id = UrlParameter.Optional });

            routes.MapRoute(
                name: "SurveyCustomers",
                url: "Surveys/{surveyId}/SurveyCustomer/{action}/{id}",
                defaults: new { controller = "SurveyCustomer", action = "Index", id = UrlParameter.Optional });

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
