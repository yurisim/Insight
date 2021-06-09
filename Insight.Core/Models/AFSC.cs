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
        /// Name of AFSC, example 3D0X4
        /// </summary>
        public string Name { get; set; }

        public int TBATasksNeeded { get; set; }

        public List<Person> Persons { get; set; }
    }
}
