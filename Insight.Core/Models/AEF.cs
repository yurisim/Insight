using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Insight.Core.Models
{
    public class AEF
    {
        [Key]
        public int Id { get; set; }

        public Person Person { get; set; }

        public Training[] Training  { get; set; }

        public DateTime[] TrainingCompleteDate { get; set; }

        public DateTime[] TrainingExpireDate { get; set; }
    }
}
