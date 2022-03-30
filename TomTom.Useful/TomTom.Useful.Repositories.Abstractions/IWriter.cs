using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TomTom.Useful.DataTypes;

namespace TomTom.Useful.Repositories.Abstractions
{
    public interface IWriter<TIdentity, TEntity> : IInserter<TEntity>, IUpdater<TEntity>, IDeleter<TEntity>
    {
    }

    public interface IInserter<TEntity>
    {
        Task<Result<object>> Insert(TEntity entity);
    }

    public interface IUpdater<TEntity>
    {
        Task<Result<object>> Update(TEntity entity);
    }

    public interface IDeleter<TIdentity>
    {
        Task<Result<object>> Delete(TIdentity identity);
    }
}
