using Microsoft.EntityFrameworkCore;
using StudentManagementSystemAPI.Models;
using System.Threading.Tasks;

namespace StudentManagementSystemAPI.DbContexts
{
    public class ApplicationDbContext : DbContext , IApplicationDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public async Task<int> SaveChanges()
        {
            return await base.SaveChangesAsync();
        }

        public DbSet<StudentModel> Students { get; set; }
        public DbSet<GradeModel> Grades { get; set; }
        public DbSet<CourseModel> Courses { get; set; }
        public DbSet<AttendanceModel> Attendance { get; set; }

        
    }
}
