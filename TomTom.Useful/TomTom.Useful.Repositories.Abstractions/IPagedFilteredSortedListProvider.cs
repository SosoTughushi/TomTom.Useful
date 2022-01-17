using System.Linq.Expressions;
using TomTom.Useful.DataTypes;

namespace TomTom.Useful.Repositories.Abstractions
{
    public interface IPagedFilteredSortedListProvider<T>
    {
        Task<PagedResult<T>> GetPagedFilteredSorted(
            Expression<Func<T, bool>> filterExpression,
            Expression<Func<T, object>> sortExpression,
            int skip, int take);


        Task<PagedResult<T>> GetPagedFilteredSortedDesc(
            Expression<Func<T, bool>> filterExpression,
            Expression<Func<T, object>> sortExpression,
            int skip, int take);
    }


}
