using System;

namespace StudentManagementSystemAPI.Models
{
    public class StudentModel
    {
        public int Id { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public DateTime Dateofbirth { get; set; }
        public string Gender { get; set; }
        public enum Attendance
        {
            Present,
            Absent
        }
    }
    class Attendance
    {
        DateTime dateofbirth;

    }
}
