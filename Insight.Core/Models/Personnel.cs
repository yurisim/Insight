using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Insight.Core.Models
{
    public class Personnel
    {
      [Key, ForeignKey("Person"), Required]
      public int PersonId { get; set; }

      public Status OverallStatus { get; set; }
    }
}
