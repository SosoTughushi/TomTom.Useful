using System.Linq.Expressions;
using TomTom.Useful.DataTypes;

namespace TomTom.Useful.Repositories.Abstractions
{
    public interface IPagedSortedListProvider<T>
    {
        Task<PagedResult<T>> GetPagedSorted(
            Expression<Func<T, object>> sortExpression, int skip, int take);

        Task<PagedResult<T>> GetPagedSortedDesc(
            Expression<Func<T, object>> sortExpression, int skip, int take);
    }


}
