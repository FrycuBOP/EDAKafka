using CQRS.Core.Domain;
using Post.Common.Events;

namespace Post.Cmd.Domain.Aggregates
{
    public class PostAggregate : AggregateRoot
    {
        private bool _active;
        private string? _author;
        private readonly Dictionary<Guid, (string, string)> _comments = [];

        public bool Active { get { return _active; } set { _active = value; } }

        public PostAggregate() { }

        public PostAggregate(Guid id, string author, string message)
        {
            RaiseEvent(new PostCreatedEvent
            {
                Id = id,
                Author = author,
                Message = message,
                DatePosted = DateTime.UtcNow
            });
        }

        public void Apply(PostCreatedEvent @event)
        {
            _id = @event.Id;
            _author = @event.Author;
            _active = true;
        }

        public void EditMessage(string message)
        {
            if (!_active)
            {
                throw new InvalidOperationException("You cannot edit the message of an inactive post");
            }
            if (string.IsNullOrEmpty(message))
            {
                throw new InvalidOperationException($"The value of {nameof(message)} cannot be null or empty. Please provide a valid {nameof(message)}.");
            }

            RaiseEvent(new MessageUpdatedEvent
            {
                Message = message,
                Id = _id,
            });
        }

        public void Apply(MessageUpdatedEvent @event) { _id = @event.Id; }

        public void LikePost()
        {
            if (!_active)
            {
                throw new InvalidOperationException("You cannot like an inactive post");
            }

            RaiseEvent(new PostLikedEvent
            {
                Id = _id
            });
        }

        public void Apply(PostLikedEvent @event) { _id = @event.Id; }

        public void AddCommnet(string comment, string userName)
        {
            if (!_active)
            {
                throw new InvalidOperationException("You cannot comment an inactive post");
            }
            if (string.IsNullOrEmpty(comment))
            {
                throw new InvalidOperationException($"The value of {nameof(comment)} cannot be null or empty. Please provide a valid {nameof(comment)}.");
            }

            RaiseEvent(new CommentAddedEvent
            {
                Id = _id,
                Comment = comment,
                UserName = userName,
                CommentId = Guid.NewGuid(),
                CommentDate = DateTime.UtcNow,
            });
        }

        public void Apply(CommentAddedEvent @event)
        {
            _id = @event.Id;
            _comments.Add(@event.CommentId, (@event.Comment, @event.UserName));
        }

        public void EditComment(Guid commentId, string comment, string userName)
        {
            if (!_active)
            {
                throw new InvalidOperationException("You cannot edit an inactive post");
            }
            if (!_comments[commentId].Item2.Equals(userName, StringComparison.CurrentCultureIgnoreCase))
            {
                throw new InvalidOperationException($"You are not allowed to edit comment that was made by another user.");

            }
            if (string.IsNullOrEmpty(comment))
            {
                throw new InvalidOperationException($"The value of {nameof(comment)} cannot be null or empty. Please provide a valid {nameof(comment)}.");
            }

            RaiseEvent(new CommentUpdatedEvent
            {
                Id = _id,
                CommentId = commentId,
                UserName = userName,
                Comment = comment,
                EditDate = DateTime.UtcNow
            });
        }

        public void Apply(CommentUpdatedEvent @event)
        {
            _id = @event.Id;
            _comments[@event.CommentId] = new(@event.Comment, @event.UserName);
        }

        public void RemoveComment(Guid commentId, string userName)
        {
            if (!_active)
            {
                throw new InvalidOperationException("You cannot remove a comment of an inactive post");
            }
            if (!_comments[commentId].Item2.Equals(userName, StringComparison.CurrentCultureIgnoreCase))
            {
                throw new InvalidOperationException($"You are not allowed to remove comment that was made by another user.");
            }

            RaiseEvent(new CommentRemovedEvent { Id = _id, CommentId = commentId });
        }

        public void Apply(CommentRemovedEvent @event)
        {
            _id = @event.Id;
            _comments.Remove(@event.CommentId);
        }

        public void DeletePost(string userName)
        {
            if (!_active)
            {
                throw new InvalidOperationException("The post has already been removed");
            }
            if (!_author!.Equals(userName, StringComparison.CurrentCultureIgnoreCase))
            {
                throw new InvalidOperationException($"You are not allowed to delete post that was made by another user.");
            }

            RaiseEvent(new PostRemovedEvent { Id = _id });
        }

        public void Apply(PostRemovedEvent @event) { _id = @event.Id; _active = false; }
    }
}
