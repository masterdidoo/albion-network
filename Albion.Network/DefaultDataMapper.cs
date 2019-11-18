using System;
using System.Collections.Generic;

namespace Albion.Network
{
    public class DefaultDataMapper : IDataMapper
    {
        public T MapFromParameters<T>(Dictionary<byte, object> parameters)
        {
            return (T)Activator.CreateInstance(typeof(T), parameters);
        }
    }
}
