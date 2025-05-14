using System.Threading.Tasks;
using GraphQL.API.DataAccess;
using GraphQL.API.DataAccess.Repository;
using GraphQL.API.DTOs;
using GraphQL.API.Schemas.Courses;
using Microsoft.EntityFrameworkCore;

namespace GraphQL.API.GraphQL
{
    public class Query
    {
        private readonly CoursesRepository _coursesRepository;

        public Query(CoursesRepository coursesRepository)
        {
            _coursesRepository = coursesRepository;
        }

        public async Task<IEnumerable<CourseType>> GetCourses()
        {
            IEnumerable<CourseDto> coursesDto = await _coursesRepository.GetAll();

            IEnumerable<CourseType> courses = coursesDto.Select(c => new CourseType()
            {
                Id = c.Id,
                Name = c.Name,
                Subject = c.Subject,
                InstructorId = c.InstructorId,
            });

            return courses;
        }

        [UsePaging(IncludeTotalCount = true, DefaultPageSize = 10)]
        [UseFiltering(typeof(CourseFilterType))]
        [UseSorting(typeof(CourseSortType))]
        public async Task<IQueryable<CourseType>> GetPaginatedCourses([Service] IDbContextFactory<ApplicationDbContext> dbContextFactory)
        {
            var dbContext = await dbContextFactory.CreateDbContextAsync();

            IQueryable<CourseType> courses = dbContext.Courses.Select(c => new CourseType()
            {
                Id = c.Id,
                Name = c.Name,
                Subject = c.Subject,
                InstructorId = c.InstructorId,
            }).AsNoTracking();

            return courses;
        }

        public async Task<CourseType> GetCourseByIdAsync(Guid id)
        {
            CourseDto courseDto = await _coursesRepository.GetById(id);

            if (courseDto is null)
            {
                throw new GraphQLException("Course not found");
            }

            CourseType courseType = new CourseType()
            {
                Id = courseDto.Id,
                Name = courseDto.Name,
                Subject = courseDto.Subject,
            };

            return courseType;
        }


        [GraphQLDeprecated("This query is deprecated")]
        public string Instructions => "test";
    }
}
