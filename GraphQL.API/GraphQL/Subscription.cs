using GraphQL.API.Schemas.Courses;
using HotChocolate.Execution;
using HotChocolate.Subscriptions;
using HotChocolate.Types;

namespace GraphQL.API.GraphQL
{
    public class Subscription
    {
        [Subscribe]
        public CourseResult CourseCreated([EventMessage] CourseResult course) => course;


        public ValueTask<ISourceStream<CourseResult>> SubscribeToCoursesUpdates(Guid courseId, [Service] ITopicEventReceiver topicEventReceiver)
        {
            string topicName = $"{courseId}_{nameof(Subscription.CourseUpdated)}";

            return topicEventReceiver.SubscribeAsync<CourseResult>(topicName);
        }

        [Subscribe(With = nameof(SubscribeToCoursesUpdates))]
        public CourseResult CourseUpdated([EventMessage] CourseResult course) => course;
    }
}
