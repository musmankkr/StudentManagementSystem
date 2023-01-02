namespace StudentManagementSystemAPI.Models
{
    public class AttendanceDetail
    {
        public StudentModel student { get; set; }
        public CourseModel course { get; set; }       
        public AttendanceModel attendance { get; set; }        
    }
}
