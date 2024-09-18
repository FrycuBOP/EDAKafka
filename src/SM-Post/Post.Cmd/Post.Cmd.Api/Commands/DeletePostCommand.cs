using CQRS.Core.Commands;

namespace Post.Cmd.Api.Commands
{
    public class DeletePostCommand : BaseCommand
    {
        public required string UserName { get; set; }
    }
}
