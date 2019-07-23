using System;
using System.Collections.Generic;
using System.Text;

namespace TomTom.Useful.Repositories.Abstractions
{
    public interface ICrud<TIdentity, TEntity>
        : IKeyValueRepository<TIdentity, TEntity>,
        IListProvider<TEntity>
    {
    }
}
