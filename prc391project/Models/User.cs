using System;
using System.Collections.Generic;

namespace prc391project.Models
{
    public partial class User
    {
        public User()
        {
            Course = new HashSet<Course>();
            CourseClass = new HashSet<CourseClass>();
        }

        public string UserId { get; set; }
        public string FullName { get; set; }
        public string Password { get; set; }
        public bool? Role { get; set; }
        public string Email { get; set; }

        public virtual ICollection<Course> Course { get; set; }
        public virtual ICollection<CourseClass> CourseClass { get; set; }
    }
}
