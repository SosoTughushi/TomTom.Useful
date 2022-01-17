using System.Linq.Expressions;
using TomTom.Useful.DataTypes;

namespace TomTom.Useful.Repositories.Abstractions
{
    public interface IPagedFilteredListProvider<T>
    {
        Task<PagedResult<T>> GetPagedFiltered(
            Expression<Func<T, bool>> filterExpression, int skip, int take);
    }


}
