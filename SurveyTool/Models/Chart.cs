using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SurveyTool.Models
{
    public class Chart
    {
        public Chart(string name,double persen)
        {
            this.name = name;
            this.persen = persen;
        }
        public string name { get; set; }
        public double persen { get; set; }
    }
}