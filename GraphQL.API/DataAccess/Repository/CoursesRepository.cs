using GraphQL.API.DTOs;
using Microsoft.EntityFrameworkCore;

namespace GraphQL.API.DataAccess.Repository
{
    public class CoursesRepository
    {
        private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;

        public CoursesRepository(IDbContextFactory<ApplicationDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<IEnumerable<CourseDto>> GetAll()
        {
            using (ApplicationDbContext context = _contextFactory.CreateDbContext())
            {
                return await context.Courses.ToListAsync();
            }
        }

        public async Task<CourseDto> GetById(Guid courseId)
        {
            using (ApplicationDbContext context = _contextFactory.CreateDbContext())
            {
                return await context.Courses.FirstOrDefaultAsync(c => c.Id == courseId);
            }
        }

        public async Task<CourseDto> Create(CourseDto course)
        {
            using (ApplicationDbContext context = _contextFactory.CreateDbContext())
            {
                context.Courses.Add(course);
                await context.SaveChangesAsync();

                return course;
            }
        }

        public async Task<CourseDto> Update(CourseDto course)
        {
            using (ApplicationDbContext context = _contextFactory.CreateDbContext())
            {
                context.Courses.Update(course);
                await context.SaveChangesAsync();

                return course;
            }
        }

        public async Task<bool> Delete(Guid id)
        {
            using (ApplicationDbContext context = _contextFactory.CreateDbContext())
            {
                CourseDto course = new CourseDto()
                {
                    Id = id
                };

                context.Courses.Remove(course);

                return await context.SaveChangesAsync() > 0;
            }
        }
    }
}
