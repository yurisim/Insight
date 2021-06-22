using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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

      /// <summary>
      /// These are the people that are assigned to this AFSC
      /// </summary>
      public List<Person> Persons { get; set; }
   }
}