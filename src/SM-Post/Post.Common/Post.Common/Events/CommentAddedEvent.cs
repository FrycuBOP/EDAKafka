using CQRS.Core.Events;

namespace Post.Common.Events
{
    public class CommentAddedEvent : BaseEvent
    {
        public CommentAddedEvent() : base(nameof(CommentAddedEvent))
        {
        }

        public Guid CommentId { get; set; }
        public required string Comment { get; set; }
        public required string UserName { get; set; }
        public DateTime CommentDate { get; set; }
    }
}
