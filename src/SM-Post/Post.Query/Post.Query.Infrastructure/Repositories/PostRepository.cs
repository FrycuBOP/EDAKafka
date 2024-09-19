using Microsoft.EntityFrameworkCore;
using Post.Query.Domain.Entity;
using Post.Query.Domain.Repositories;
using Post.Query.Infrastructure.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Post.Query.Infrastructure.Repositories
{
    public class PostRepository : IPostRepository
    {
        private readonly DatabaseContextFactory _contextFactory;

        public PostRepository(DatabaseContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task CreateAsync(PostEntity post)
        {
            using DatabaseContext context = _contextFactory.CreateDbContext();
            context.Posts.Add(post);
            _ = await context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid postId)
        {
            using DatabaseContext context = _contextFactory.CreateDbContext();
            var post = await GetByIdAsync(postId);
            if (post != null)
            {
                context.Posts.Remove(post);
                _ = await context.SaveChangesAsync();
            }
        }

        public async Task<PostEntity?> GetByIdAsync(Guid postId)
        {
            using DatabaseContext context = _contextFactory.CreateDbContext();
            return await context.Posts.Include(x => x.Comments).FirstOrDefaultAsync(x => x.PostId == postId);
        }

        public async Task<List<PostEntity>> ListAllAsync()
        {
            using DatabaseContext context = _contextFactory.CreateDbContext();
            return await context.Posts.AsNoTracking().Include(_ => _.Comments).AsNoTracking().ToListAsync();
        }

        public async Task<List<PostEntity>> ListByAuthorAsync(string author)
        {
            using DatabaseContext context = _contextFactory.CreateDbContext();
            return await context.Posts.AsNoTracking().Include(_ => _.Comments).AsNoTracking().Where(x=>x.Author == author).ToListAsync();
        }

        public async Task<List<PostEntity>> ListWithComments()
        {
            using DatabaseContext context = _contextFactory.CreateDbContext();
            return await context.Posts.AsNoTracking().Include(_ => _.Comments).Where(x=>x.Comments != null && x.Comments.Any()).AsNoTracking().ToListAsync();
        }

        public async Task<List<PostEntity>> ListWithLikeAsync(int numberOfLikes)
        {
            using DatabaseContext context = _contextFactory.CreateDbContext();
            return await context.Posts.AsNoTracking().Include(_ => _.Comments).Where(x => x.Likes > 0).AsNoTracking().ToListAsync();
        }

        public async Task UpdateAsync(PostEntity post)
        {
            using DatabaseContext context = _contextFactory.CreateDbContext();
            context.Posts.Update(post);
            _ = await context.SaveChangesAsync(); 
        }
    }
}
