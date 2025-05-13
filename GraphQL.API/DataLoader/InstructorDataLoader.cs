using GraphQL.API.DataAccess.Repository;
using GraphQL.API.DTOs;

namespace GraphQL.API.DataLoader
{
    public class InstructorDataLoader : BatchDataLoader<Guid, InstructorDto>
    {
        private readonly InstructorsRepository _instructorsRepository;
        public InstructorDataLoader(InstructorsRepository instructorsRepository, IBatchScheduler batchScheduler, DataLoaderOptions options) : base(batchScheduler, options)
        {
            _instructorsRepository = instructorsRepository;
        }

        protected override async Task<IReadOnlyDictionary<Guid, InstructorDto>> LoadBatchAsync(IReadOnlyList<Guid> keys, CancellationToken cancellationToken)
        {
            IEnumerable<InstructorDto> instructors = await _instructorsRepository.GetManyByIds(keys);

            return instructors.ToDictionary(i => i.Id);
        }
    }
}
