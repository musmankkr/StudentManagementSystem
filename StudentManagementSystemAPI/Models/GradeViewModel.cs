using System;

namespace StudentManagementSystemAPI.Models
{
    public class GradeViewModel
    {
        public string Grade { get; set; }

        public string StudentName { get; set; }

        public int StudentId { get; set; }

        public int CourseId { get; set; }

        public string CourseName { get; set; }
    }
}
