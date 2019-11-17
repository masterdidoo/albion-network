using System.Collections.Generic;

namespace Albion.Network
{
    public interface IDataDeserializer
    {
        T Deserialize<T>(Dictionary<byte, object> parameters);
    }
}
