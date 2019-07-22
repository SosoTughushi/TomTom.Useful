using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TomTom.Useful.Repositories.Abstractions
{
    public interface IEntityByKeyProvider<TIdentity, TEntity>
    {
        Task<TEntity> Get(TIdentity identity);
    }
}
