using Microsoft.EntityFrameworkCore;
using StudentManagementSystemAPI.Models;
using System.Threading.Tasks;

namespace StudentManagementSystemAPI.DbContexts
{
    public interface IApplicationDbContext
    {
        Task<int> SaveChanges();

        public DbSet<StudentModel> Students { get; set; }
        public DbSet<GradeModel> Grades { get; set; }
        public DbSet<CourseModel> Courses { get; set; }
        public DbSet<AttendanceModel> Attendance { get; set; }
    }
}
