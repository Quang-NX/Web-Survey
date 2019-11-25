using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace SurveyTool.Models
{
    public class Response
    {
        [Key]
        public int Id { get; set; }

        public int SurveyId { get; set; }

        public Survey Survey { get; set; }

        public string CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        public ICollection<Answer> Answers { get; set; }

        public int GetQuestionCount()
        {
            return Answers == null ? 0 : Answers.Count();
        }

        public int GetAnswerCount()
        {
            return Answers == null ? 0 : Answers.Sum(x => x.Score);
        }

        public double CalculateScore(int totalQuestion=0)
        {
            var answers = GetAnswerCount();

            if (totalQuestion == 0 || answers == 0)
                return 0.0;

            return (double)answers / (double)totalQuestion;
        }

        public int CusId { get; set; }
        [ForeignKey("CusId")]
        public virtual Customer Customer { get; set; }

    }
}