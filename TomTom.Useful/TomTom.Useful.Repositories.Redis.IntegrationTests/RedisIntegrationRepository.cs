using System;
using System.Collections.Generic;
using System.Text;

namespace TomTom.Useful.Repositories.Redis.IntegrationTests
{
    public class RedisIntegrationRepository : RedisRepository<RedisEntityForIntegration>
    {
        public RedisIntegrationRepository(
            IRedisStorage<RedisEntityForIntegration> storage)
            : base(storage)
        {
        }
    }
}
