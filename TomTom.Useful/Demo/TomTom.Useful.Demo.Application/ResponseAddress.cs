using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TomTom.Useful.Demo.Application
{
    public class ResponseAddress
    {
        public ResponseAddress(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; }
    }
}
