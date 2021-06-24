using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Insight.Core.Models
{
    public class Personnel
    {
        [Key]
        public long Id { get; set; }

        public Status OverallStatus { get; set; }

        public Person Person { get; set; }
    }
}
