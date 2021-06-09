using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Insight.Core.Models
{
   public class Org
   {
      [Key]
      public int Id { get; set; }

      public string Name { get; set; }

      public List<Org> SubOrgs { get; set; }
   }
}