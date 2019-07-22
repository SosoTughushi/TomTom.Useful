using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TomTom.Useful.DataTypes;

namespace TomTom.Useful.Repositories.Abstractions
{
    public interface IWriter<TIdentity, TEntity>
    {
        Task<Result<object>> Insert(TEntity entity);

        Task<Result<object>> Update(TEntity entity);

        Task<Result<object>> Delete(TIdentity identity);
    }
}
