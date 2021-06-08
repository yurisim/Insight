using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Insight.Core.Models
{
    public class MiscTraining
    {
        /// <summary>
        ///     This the primary Key for the User class. It is NOT an int because DoDIDs
        ///     are 10 digits long and ints won't cover DoDIDs larger than 2.1 bil.
        /// </summary>
        [Key]
        public long Id { get; set; }

        public Person Person { get; set; }

    }
}
