using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Insight.Core.Models
{
    public class PEX
    {
        [Key]
        public long Id { get; set; }

        public string Name { get; set; }
    }
}
