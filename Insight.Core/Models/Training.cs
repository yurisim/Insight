using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Insight.Core.Models
{
    public class Training
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public int Interval { get; set; }

    }
}
