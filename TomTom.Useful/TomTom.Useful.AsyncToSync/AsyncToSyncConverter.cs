using System;
using System.Collections.Generic;
using System.Text;

namespace TomTom.Useful.AsyncToSync
{
    public class AsyncToSyncConverter<TKey, T> : AsyncToSyncConverterBase<TKey, T>
    {
        private readonly Func<T, TKey> keyExtractor;

        public AsyncToSyncConverter(int timeoutInMs, Func<T, TKey> keyExtractor) : base(timeoutInMs)
        {
            this.keyExtractor = keyExtractor;
        }

        protected override TKey ExtractKey(T item)
        {
            return this.keyExtractor(item);
        }
    }
}
