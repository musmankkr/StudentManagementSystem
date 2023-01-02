using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentManagementSystemAPI.Models
{
    public class AttendanceViewModel
    {
        public string Status { get; set; }

        public string StudentName { get; set; }

        public int StudentId { get; set; }

        public DateTime Date { get; set; }

        public int CourseId { get; set; }

        public string CourseName { get; set; }
    }
}
