using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Insight.Core.Models
{
    public class PEX
    {
        [Key]
        public int Id { get; set; }

        // Not sure, does each PEX account belong to an organization?
        public Organization Organization { get; set; }

        public string Name { get; set; }

        public List<Person> Persons { get; set; }
    }
}
