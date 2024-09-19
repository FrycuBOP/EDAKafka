using Post.Common.DTO;
using Post.Query.Domain.Entity;

namespace Post.Query.Api.DTOs
{
    public class PostLookupResponse : BaseResponse
    {
        public List<PostEntity>? Posts { get; set; }
    }
}
