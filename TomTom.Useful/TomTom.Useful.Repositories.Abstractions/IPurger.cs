using System.Threading.Tasks;
using TomTom.Useful.DataTypes;

namespace TomTom.Useful.Repositories.Abstractions
{
    public interface IPurger<T>
    {
        Task<Result<object>> Purge();
    }
}
