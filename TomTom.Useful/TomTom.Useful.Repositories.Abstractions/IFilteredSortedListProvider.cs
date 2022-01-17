using System.Linq.Expressions;

namespace TomTom.Useful.Repositories.Abstractions
{
    public interface IFilteredSortedListProvider<T>
    {
        Task<IEnumerable<T>> GetFilteredSorted(
            Expression<Func<T, bool>> filterExpression,
            Expression<Func<T,object>> sortExpression);

        Task<IEnumerable<T>> GetFilteredSortedDesc(
            Expression<Func<T, bool>> filterExpression,
            Expression<Func<T, object>> sortExpression);
    }


}
