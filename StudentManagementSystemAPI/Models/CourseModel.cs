using System.ComponentModel.DataAnnotations;

namespace StudentManagementSystemAPI.Models
{
    public class CourseModel
    {
        [Key]
        public int CourseId { get; set; }

        public string CourseCode { get; set; }

        public string CourseName { get; set; }

        public double CourseCredit { get; set; }

        public string TeacherName { get; set; }
    }
}
