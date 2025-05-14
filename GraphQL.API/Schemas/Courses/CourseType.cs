using System.Threading.Tasks;
using GraphQL.API.DataAccess.Repository;
using GraphQL.API.DataLoader;
using GraphQL.API.DTOs;
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

        [IsProjected(true)]
        public Guid InstructorId { get; set; }

        [GraphQLNonNullType]
        public async Task<InstructorType> Instructor([Service] InstructorDataLoader instructorDataLoader)
        {
            InstructorDto instructorDto = await instructorDataLoader.LoadAsync(InstructorId, CancellationToken.None);

            return new InstructorType()
            {
                Id = instructorDto.Id,
                FirstName = instructorDto.FirstName,
                LastName = instructorDto.LastName,
                Salary = instructorDto.Salary,
            };
        }
        public IEnumerable<StudentType> Students { get; set; }
    }
}
