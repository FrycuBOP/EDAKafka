using CQRS.Core.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Post.Common.Events
{
    public class PostCreatedEvent : BaseEvent
    {
        public PostCreatedEvent() : base(nameof(PostCreatedEvent))
        {
        }

        public required string Author { get; set; }
        public required string Message { get; set; }
        public DateTime DatePosted { get; set; }
    }
}
