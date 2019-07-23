using System;
using System.Collections.Generic;
using System.Text;

namespace TomTom.Useful.Repositories.Redis.IntegrationTests
{
    public class RedisEntityForIntegration : RedisEntity
    {
        public string Username { get; set; }

        public int Age { get; set; }

        public string Email { get; set; }
    }
}
