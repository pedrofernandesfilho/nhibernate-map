using ConsoleApp1.Domain;
using NHibernate;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApplication1.Data
{
    public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : BaseEntity
    {
        protected readonly ISession session;

        public BaseRepository(ISession session) => this.session = session;

        public virtual async Task<TEntity?> GetByIdAsync(long id)
        {
            using var trans = session.BeginTransaction();
            var entity = await session.GetAsync<TEntity?>(id);
            await trans.CommitAsync();
            return entity;
        }

        public virtual async Task InsertAsync(TEntity entity)
        {
            using var trans = session.BeginTransaction();
            await session.SaveAsync(entity);
            await trans.CommitAsync();
        }

        public async Task InsertAsync(IEnumerable<TEntity> entities)
        {
            using var trans = session.BeginTransaction();
            foreach (var entity in entities)
                await session.SaveAsync(entity);
            await trans.CommitAsync();
        }
    }
}
