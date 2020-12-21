using System;
using System.Collections.Generic;

namespace prc391project.Models
{
    public partial class Room
    {
        public Room()
        {
            Course = new HashSet<Course>();
        }

        public int RoomId { get; set; }
        public string RoomName { get; set; }
        public bool? IsUsed { get; set; }

        public virtual ICollection<Course> Course { get; set; }
    }
}
