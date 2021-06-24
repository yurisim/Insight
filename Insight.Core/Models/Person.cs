using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Insight.Core.Models
{
   public class Person
   {
      [Key]
      public long Id { get; set; }

      public string FirstName { get; set; }

      public string LastName { get; set; }

      public string Name => $"{LastName}, {FirstName}";

      public string Phone { get; set; }

      public long DoDID { get; set; }

      public string SSN { get; set; }

      public string DateOnStation { get; set; }

      public string Comments { get; set; }

      public Medical Medicals { get; set; }

      public AFSC AFSC { get; set; }

      public Org Organization { get; set; }

      public PEX PEX { get; set; }

      public List<CourseInstance> CourseInstances { get; set; }
   }
}