using System.Linq.Expressions;

namespace TomTom.Useful.Repositories.Abstractions
{
    public interface IFilteredListProvider<T>
    {
        Task<IEnumerable<T>> GetFiltered(Expression<Func<T, bool>> filterExpression);
    }


}
