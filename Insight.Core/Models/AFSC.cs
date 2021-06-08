using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Insight.Core.Models
{
    public class AFSC
    {
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Name of AFSC
        /// </summary>
        public string Name { get; set; }

        public int TBATasksNeeded { get; set; }
    }
}
