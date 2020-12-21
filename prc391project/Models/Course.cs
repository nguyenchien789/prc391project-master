using System;
using System.Collections.Generic;

namespace prc391project.Models
{
    public partial class Course
    {
        public Course()
        {
            CourseClass = new HashSet<CourseClass>();
        }

        public int CourseId { get; set; }
        public string UserId { get; set; }
        public string SubjectId { get; set; }
        public int? RoomId { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }

        public virtual Room Room { get; set; }
        public virtual Subject Subject { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<CourseClass> CourseClass { get; set; }
    }
}
