using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
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
    public class CommentRepository : ICommentRepository
    {
        private readonly DatabaseContextFactory _databaseContextFactory;

        public CommentRepository(DatabaseContextFactory databaseContextFactory)
        {
            _databaseContextFactory = databaseContextFactory;
        }

        public async Task CrateAsync(CommentEntity comment)
        {
            using DatabaseContext context = _databaseContextFactory.CreateDbContext();
            context.Comments.Add(comment);
            _ = await context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid commentId)
        {
            using DatabaseContext context = _databaseContextFactory.CreateDbContext();
            var comment = await GetByIdAsync(commentId);
            context.Comments.Remove(comment);
            _ = await context.SaveChangesAsync();

        }

        public async Task<CommentEntity?> GetByIdAsync(Guid commentId)
        {
            using DatabaseContext context = _databaseContextFactory.CreateDbContext();
            return await context.Comments.FirstOrDefaultAsync(x=>x.CommentId == commentId);
        }

        public async Task UpdateAsync(CommentEntity comment)
        {
            using DatabaseContext context = _databaseContextFactory.CreateDbContext();
            context.Comments.Update(comment);
            _ = await context.SaveChangesAsync();
        }
    }
}
