using System;
using System.Collections.Generic;

namespace prc391project.Models
{
    public partial class Subject
    {
        public Subject()
        {
            Course = new HashSet<Course>();
        }

        public string SubjectId { get; set; }
        public string SubjectName { get; set; }
        public string Description { get; set; }

        public virtual ICollection<Course> Course { get; set; }
    }
}
