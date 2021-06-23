using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Insight.Core.Models
{
    public class AEF
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }
        public string CourseCount { get; set; }
        public string DRCStatusForEmail { get; set; }
        public string PayGrade { get; set; }
        public string AEFI { get; set; }
        public string Unit { get; set; }
        public string PASCode { get; set; }
        public string AFSC { get; set; }
        public string Gender { get; set; }
        public string DutyStatus { get; set; }
        public string Personnel { get; set; }
        public string Medical { get; set; }
        public string Training { get; set; }
        public string HasAEFAccount { get; set; }
        public string VisitedAEFOnline { get; set; }
        public string ChecklistStatus { get; set; }
        public string ModeTip { get; set; }
    }
}
