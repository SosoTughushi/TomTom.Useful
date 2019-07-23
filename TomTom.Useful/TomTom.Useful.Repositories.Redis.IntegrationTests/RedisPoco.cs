using System;
using System.Collections.Generic;
using System.Text;

namespace TomTom.Useful.Repositories.Redis.IntegrationTests
{
    public class RedisPoco
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public double Prioriy { get; set; }



        public InnerPoco Inner { get; set; }

        public class InnerPoco
        {
            public string Child { get; set; }

            public int Rand { get; set; }
        }
    }
}
