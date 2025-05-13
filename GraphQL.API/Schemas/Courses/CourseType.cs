using GraphQL.API.Models;
using GraphQL.API.Schemas.Instructors;
using GraphQL.API.Schemas.Students;

namespace GraphQL.API.Schemas.Courses
{
    public class CourseType
    {
        public Guid Id { get; set; }

        public string Name { get; set; }
        public Subject Subject { get; set; }

        [GraphQLNonNullType]
        public InstructorType Instructor { get; set; }
        public IEnumerable<StudentType> Students { get; set; }

        public string Description()
        {
            return $"{Name}: This is a course.";
        }
    }
}
