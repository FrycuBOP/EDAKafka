using CQRS.Core.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Post.Common.Events
{
    public class CommentUpdatedEvent : BaseEvent
    {
        public CommentUpdatedEvent() : base(nameof(CommentAddedEvent))
        {
        }

        public Guid CommentId { get; set; }
        public required string Comment { get; set; }
        public required string UserName { get; set; }
        public DateTime EditDate { get; set; }
    }
}
