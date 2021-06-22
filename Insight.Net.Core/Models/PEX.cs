using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Insight.Net.Core.Models
{
   public class PEX
   {
      [Key]
      public int Id { get; set; }

      // Not sure, does each PEX account belong to an organization?
      public Org Organization { get; set; }

      public string Name { get; set; }

      public List<Person> Persons { get; set; }
   }
}