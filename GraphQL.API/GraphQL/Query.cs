using System.Threading.Tasks;
using Bogus;
using GraphQL.API.DataAccess.Repository;
using GraphQL.API.DTOs;
using GraphQL.API.Models;
using GraphQL.API.Schemas.Courses;
using GraphQL.API.Schemas.Instructors;
using GraphQL.API.Schemas.Students;

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
