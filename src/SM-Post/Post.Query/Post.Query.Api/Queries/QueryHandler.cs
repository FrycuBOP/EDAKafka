using Post.Query.Domain.Entity;
using Post.Query.Domain.Repositories;

namespace Post.Query.Api.Queries
{
    public class QueryHandler : IQueryHandler
    {
        private readonly IPostRepository _repository;

        public QueryHandler(IPostRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<PostEntity>> HandleAsync(FindAllPostQuery query)
        {
            return await _repository.ListAllAsync();
        }

        public async Task<List<PostEntity>> HandleAsync(FindPostByIdQuery query)
        {
            return [await _repository.GetByIdAsync(query.Id)];
        }

        public async Task<List<PostEntity>> HandleAsync(FindPostsByAuthorQuery query)
        {
            return await _repository.ListByAuthorAsync(query.Author); 
        }

        public async Task<List<PostEntity>> HandleAsync(FindPostsWithCommentsQuery query)
        {
            return await _repository.ListWithComments();
        }

        public async Task<List<PostEntity>> HandleAsync(FindPostsWithLikesQuery query)
        {
            return await _repository.ListWithLikeAsync(query.NumberOfLikes);
        }
    }
}
