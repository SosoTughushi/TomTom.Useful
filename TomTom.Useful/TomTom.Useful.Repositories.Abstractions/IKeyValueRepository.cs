using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TomTom.Useful.DataTypes;

namespace TomTom.Useful.Repositories.Abstractions
{
    public interface IKeyValueRepository<TIdentity, TEntity>
        : IWriter<TIdentity, TEntity>,
        IEntityByKeyProvider<TIdentity, TEntity>
    {
    }
}
