using System;
using System.Collections.Generic;

namespace Albion.Network
{
    public class DefaultDataDeserializer : IDataDeserializer
    {
        public T Deserialize<T>(Dictionary<byte, object> parameters)
        {
            return (T)Activator.CreateInstance(typeof(T), parameters);
        }
    }
}
