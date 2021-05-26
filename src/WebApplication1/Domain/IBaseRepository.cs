using System.Collections.Generic;
using System.Threading.Tasks;

namespace ConsoleApp1.Domain
{
    public interface IBaseRepository<TEntity> where TEntity : BaseEntity
    {
        Task<TEntity?> GetByIdAsync(long id);

        Task InsertAsync(IEnumerable<TEntity> entities);

        Task InsertAsync(TEntity entity);
    }
}
