using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TomTom.Useful.DataTypes;

namespace TomTom.Useful.Repositories.Abstractions
{
    public interface IListProvider<T>
    {
        Task<IEnumerable<T>> GetAll();
    }

    public interface IFilteredListProvider<T>
    {
        Task<IEnumerable<T>> GetFiltered(Expression<Func<T, bool>> filterExpression);
    }

    public interface ISortedListProvider<T>
    {
        Task<IEnumerable<T>> GetSorted(Expression<Func<T, object>> sortExpression);

        Task<IEnumerable<T>> GetSortedDesc(Expression<Func<T, object>> sortExpression);
    }

    public interface IFilteredSortedListProvider<T>
    {
        Task<IEnumerable<T>> GetFilteredSorted(
            Expression<Func<T, bool>> filterExpression,
            Expression<Func<T,object>> sortExpression);

        Task<IEnumerable<T>> GetFilteredSortedDesc(
            Expression<Func<T, bool>> filterExpression,
            Expression<Func<T, object>> sortExpression);
    }

    public interface IPagedListProvider<T>
    {
        Task<PagedResult<T>> GetPaged(int skip, int take);
    }

    public interface IPagedFilteredListProvider<T>
    {
        Task<PagedResult<T>> GetPagedFiltered(
            Expression<Func<T, bool>> filterExpression, int skip, int take);
    }

    public interface IPagedSortedListProvider<T>
    {
        Task<PagedResult<T>> GetPagedSorted(
            Expression<Func<T, object>> sortExpression, int skip, int take);

        Task<PagedResult<T>> GetPagedSortedDesc(
            Expression<Func<T, object>> sortExpression, int skip, int take);
    }

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
