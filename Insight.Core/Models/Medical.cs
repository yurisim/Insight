using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Insight.Core.Models
{
    public class Medical
    {
        [Key,ForeignKey("Person"),Required]
        public int PersonId { get; set; }

        public Person Person { get; set; }

        public Status OverallStatus { get; set; }

        public Status Dental { get; set; }

        public Status Immunizations { get; set; }

        public Status Lab { get; set; }

        public Status PHA { get; set; }
    }
}