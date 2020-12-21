using System;
using System.Collections.Generic;

namespace prc391project.Models
{
    public partial class CourseClass
    {
        public int CourseId { get; set; }
        public string UserId { get; set; }

        public virtual Course Course { get; set; }
        public virtual User User { get; set; }
    }
}
