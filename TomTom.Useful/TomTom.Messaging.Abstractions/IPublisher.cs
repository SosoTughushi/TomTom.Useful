using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TomTom.Useful.Messaging
{
    public interface IPublisher<T> where T: IMessage
    {
        Task Publish(T message);
    }
}
