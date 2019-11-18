using System.Collections.Generic;

namespace Albion.Network
{
    public interface IDataMapper
    {
        T MapFromParameters<T>(Dictionary<byte, object> parameters);
    }
}
