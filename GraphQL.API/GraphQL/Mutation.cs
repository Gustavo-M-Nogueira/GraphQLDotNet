using GraphQL.API.Schemas.Courses;
using HotChocolate.Subscriptions;

namespace GraphQL.API.GraphQL
{
    public class Mutation
    {
        private readonly List<CourseResult> _courses;

        public Mutation()
        {
            _courses = new List<CourseResult>();
        }

        public async Task<CourseResult> CreateCourseAsync(CourseInputType input, [Service] ITopicEventSender topicEventSender)
        {
            CourseResult courseType = new CourseResult()
            {
                Id = Guid.NewGuid(),
                Name = input.Name,
                Subject = input.Subject,
                InstructorId = input.InstructorId,
            };

            _courses.Add(courseType);
            await topicEventSender.SendAsync(nameof(Subscription.CourseCreated), courseType);

            return courseType;
        }

        public async Task<CourseResult> UpdateCourseAsync(Guid id, CourseInputType input, [Service] ITopicEventSender topicEventSender)
        {
            CourseResult course = _courses.FirstOrDefault(c => c.Id == id);

            if (course == null)
            {
                throw new GraphQLException(new Error("Course not found.", "COURSE_NOT_FOUND"));
            }


            course.Name = input.Name;
            course.Subject = input.Subject;
            course.InstructorId = input.InstructorId;

            string updateCourseTopic = $"{course.Id}_{nameof(Subscription.CourseUpdated)}";
            await topicEventSender.SendAsync(updateCourseTopic, course);

            return course;
        }

        public bool DeleteCourse(Guid id)
        {
            return (_courses.RemoveAll(c => c.Id == id) >= 1);
        }
    }
}
