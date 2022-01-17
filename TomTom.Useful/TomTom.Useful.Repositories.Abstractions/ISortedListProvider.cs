using System.Linq.Expressions;

namespace TomTom.Useful.Repositories.Abstractions
{
    public interface ISortedListProvider<T>
    {
        Task<IEnumerable<T>> GetSorted(Expression<Func<T, object>> sortExpression);

        Task<IEnumerable<T>> GetSortedDesc(Expression<Func<T, object>> sortExpression);
    }


}
