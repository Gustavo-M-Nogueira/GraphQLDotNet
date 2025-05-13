using GraphQL.API.DTOs;
using Microsoft.EntityFrameworkCore;

namespace GraphQL.API.DataAccess
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<CourseDto> Courses { get; set; }
        public DbSet<InstructorDto> Instructors { get; set; }
        public DbSet<StudentDto> Students { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            
        }
    }
}
