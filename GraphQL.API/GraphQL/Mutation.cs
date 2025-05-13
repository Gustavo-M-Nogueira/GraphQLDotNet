using Bogus.DataSets;
using GraphQL.API.Schemas.Courses;

namespace GraphQL.API.GraphQL
{
    public class Mutation
    {
        private readonly List<CourseResult> _courses;

        public Mutation()
        {
            _courses = new List<CourseResult>();
        }

        public CourseResult CreateCourse(CourseInputType input)
        {
            CourseResult courseType = new CourseResult()
            {
                Id = Guid.NewGuid(),
                Name = input.Name,
                Subject = input.Subject,
                InstructorId = input.InstructorId,
            };

            _courses.Add(courseType);

            return courseType;
        }

        public CourseResult UpdateCourse(Guid id, CourseInputType input)
        {
            CourseResult course = _courses.FirstOrDefault(c => c.Id == id);

            if (course == null)
            {
                throw new GraphQLException(new Error("Course not found.", "COURSE_NOT_FOUND"));
            }


            course.Name = input.Name;
            course.Subject = input.Subject;
            course.InstructorId = input.InstructorId;

            return course;
        }

        public bool DeleteCourse(Guid id)
        {
            return (_courses.RemoveAll(c => c.Id == id) >= 1);
        }
    }
}
