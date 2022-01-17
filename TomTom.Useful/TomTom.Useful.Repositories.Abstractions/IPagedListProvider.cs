using TomTom.Useful.DataTypes;

namespace TomTom.Useful.Repositories.Abstractions
{
    public interface IPagedListProvider<T>
    {
        Task<PagedResult<T>> GetPaged(int skip, int take);
    }


}
