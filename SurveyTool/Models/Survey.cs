using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using Microsoft.AspNet.Identity.EntityFramework;
using Newtonsoft.Json;

namespace SurveyTool.Models
{
    public class Survey
    {
        public Survey()
        {
            Questions = new List<Question>();
            Responses = new List<Response>();
        }

        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        [ForeignKey("ApplicationUser")]
        public string UserId { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public List<Question> Questions { get; set; }

        public List<Response> Responses { get; set; }

        public bool RequiredEmail { get; set; }
        public bool IsActive
        {
            get { return StartDate < DateTime.Now && EndDate > DateTime.Now; }
        }

        public string ToJson()
        {
            var js = JsonSerializer.Create(new JsonSerializerSettings());
            var jw = new StringWriter();
            js.Serialize(jw, this);
            return jw.ToString();
        }
    }
}