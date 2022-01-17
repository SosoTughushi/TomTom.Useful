using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TomTom.Useful.Repositories.Abstractions
{
    public interface IListProvider<T>
    {
        Task<IEnumerable<T>> GetAll();
    }
}
