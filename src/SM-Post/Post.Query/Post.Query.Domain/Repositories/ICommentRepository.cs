using Post.Query.Domain.Entity;

namespace Post.Query.Domain.Repositories
{
    public interface ICommentRepository
    {
        Task CrateAsync(CommentEntity comment);
        Task UpdateAsync(CommentEntity comment);
        Task<CommentEntity> GetByIdAsync(Guid commentId);
        Task DeleteAsync(Guid commentId);
    }
}
