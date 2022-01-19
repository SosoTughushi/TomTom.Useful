using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TomTom.Useful.EventSourcing
{
    public interface IEmitsEvent<T> where T: Event
    {
        void Apply(T @event);
    }
}
