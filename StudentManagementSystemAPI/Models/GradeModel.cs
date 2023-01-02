using System.ComponentModel.DataAnnotations;

namespace StudentManagementSystemAPI.Models
{
    public class GradeModel
    {
        [Key]
        public int Id { get; set; }
        public int studentId { get; set; }
        public int courseId { get; set; }
        public string Grade { get; set; }
                
    }
}
