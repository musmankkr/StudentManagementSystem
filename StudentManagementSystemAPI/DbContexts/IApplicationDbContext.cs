using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace StudentManagementSystemAPI.DbContexts
{
    public interface IApplicationDbContext
    {
        Task<int> SaveChanges();
    }
}
