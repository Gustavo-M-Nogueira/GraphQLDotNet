using System.Threading.Tasks;
using GraphQL.API.DTOs;
using Microsoft.EntityFrameworkCore;

namespace GraphQL.API.DataAccess.Repository
{
    public class InstructorsRepository
    {
        private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;

        public InstructorsRepository(IDbContextFactory<ApplicationDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }
        public async Task<IEnumerable<InstructorDto>> GetAll()
        {
            using (ApplicationDbContext context = _contextFactory.CreateDbContext())
            {
                return await context.Instructors.ToListAsync();
            }
        }

        public async Task<InstructorDto> GetById(Guid instructorId)
        {
            using (ApplicationDbContext context = _contextFactory.CreateDbContext())
            {
                return await context.Instructors.FirstOrDefaultAsync(i => i.Id == instructorId);
            }
        }

        internal async Task<IEnumerable<InstructorDto>> GetManyByIds(IReadOnlyList<Guid> instructorsIds)
        {
            using (ApplicationDbContext context = _contextFactory.CreateDbContext())
            {
                return await context.Instructors
                    .Where(i => instructorsIds.Contains(i.Id))
                    .ToListAsync();
            }
        }

        public async Task<InstructorDto> Create(InstructorDto instructor)
        {
            using (ApplicationDbContext context = _contextFactory.CreateDbContext())
            {
                context.Instructors.Add(instructor);
                await context.SaveChangesAsync();

                return instructor;
            }
        }

        public async Task<InstructorDto> Update(InstructorDto instructor)
        {
            using (ApplicationDbContext context = _contextFactory.CreateDbContext())
            {
                context.Instructors.Update(instructor);
                await context.SaveChangesAsync();

                return instructor;
            }
        }

        public async Task<bool> Delete(Guid id)
        {
            using (ApplicationDbContext context = _contextFactory.CreateDbContext())
            {
                InstructorDto instructor = new InstructorDto()
                {
                    Id = id
                };

                context.Instructors.Remove(instructor);

                return await context.SaveChangesAsync() > 0;
            }
        }
    }
}
