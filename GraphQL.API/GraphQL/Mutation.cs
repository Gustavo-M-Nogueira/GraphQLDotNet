using System.Security.Claims;
using FirebaseAdminAuthentication.DependencyInjection.Models;
using GraphQL.API.DataAccess.Repository;
using GraphQL.API.DTOs;
using GraphQL.API.Schemas.Courses;
using GraphQL.API.Schemas.Instructors;
using HotChocolate.Authorization;
using HotChocolate.Subscriptions;

namespace GraphQL.API.GraphQL
{
    [Authorize]
    public class Mutation
    {
        private readonly CoursesRepository _coursesRepository;
        private readonly InstructorsRepository _instructorsRepository;

        public Mutation(CoursesRepository coursesRepository, InstructorsRepository instructorsRepository)
        {
            _coursesRepository = coursesRepository;
            _instructorsRepository = instructorsRepository;
        }

        public async Task<CourseResult> CreateCourseAsync(
            CourseInputType input, 
            [Service] ITopicEventSender topicEventSender,
            ClaimsPrincipal claimsPrincipal)
        {
            string userID = claimsPrincipal.FindFirstValue(FirebaseUserClaimType.ID);
            string email = claimsPrincipal.FindFirstValue(FirebaseUserClaimType.EMAIL);
            string username = claimsPrincipal.FindFirstValue(FirebaseUserClaimType.USERNAME);
            string verified = claimsPrincipal.FindFirstValue(FirebaseUserClaimType.EMAIL_VERIFIED);

            CourseDto courseDto = new CourseDto()
            {
                Name = input.Name,
                Subject = input.Subject,
                InstructorId = input.InstructorId,
            };

            courseDto = await _coursesRepository.Create(courseDto);

            CourseResult courseType = new CourseResult()
            {
                Id = courseDto.Id,
                Name = courseDto.Name,
                Subject = courseDto.Subject,
                InstructorId = courseDto.InstructorId,
            };

            await topicEventSender.SendAsync(nameof(Subscription.CourseCreated), courseType);

            return courseType;
        }

        public async Task<InstructorResult> CreateInstructorAsync(InstructorInputType input)
        {
            InstructorDto instructorDto = new InstructorDto()
            {
                Id = Guid.NewGuid(),
                FirstName = input.FirstName,
                LastName = input.LastName,
                Salary = input.Salary,
            };

            instructorDto = await _instructorsRepository.Create(instructorDto);

            InstructorResult instructor = new InstructorResult()
            {
                Id = instructorDto.Id,
                FirstName = instructorDto.FirstName,
                LastName = instructorDto.LastName,
                Salary = instructorDto.Salary,
            };

            return instructor;
        }

        public async Task<CourseResult> UpdateCourseAsync(Guid id, CourseInputType input, [Service] ITopicEventSender topicEventSender)
        {
            CourseDto courseDto = new CourseDto()
            {
                Id = id,
                Name = input.Name,
                Subject = input.Subject,
                InstructorId = input.InstructorId,
            };

            courseDto = await _coursesRepository.Update(courseDto);

            CourseResult course = new CourseResult()
            {
                Id = courseDto.Id,
                Name = courseDto.Name,
                Subject = courseDto.Subject,
                InstructorId = courseDto.InstructorId,
            };

            string updateCourseTopic = $"{course.Id}_{nameof(Subscription.CourseUpdated)}";
            await topicEventSender.SendAsync(updateCourseTopic, course);

            return course;
        }

        public async Task<bool> DeleteCourse(Guid id)
        {
            return await _coursesRepository.Delete(id);
        }
    }
}
