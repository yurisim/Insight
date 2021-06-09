using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Insight.Core.Models
{
    public class Organization
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public List<Organization> ChildOrgs { get; set; }
    }
}
