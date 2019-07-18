using System;
using System.Collections.Generic;
using System.Linq;

namespace TomTom.Useful.DataTypes
{
    public class PagedResult<T>
    {
        public PagedResult(IEnumerable<T> data, int offset, int totalSize)
        {
            Offset = offset;
            TotalSize = totalSize;
            Data = data;
        }

        public int Offset { get; }

        public int TotalSize { get; }

        public IEnumerable<T> Data { get; }

        public PagedResult<TNew> Convert<TNew>(Func<T, TNew> selector)
        {
            return new PagedResult<TNew>(this.Data.Select(selector), this.Offset, this.TotalSize);
        }
    }
}
