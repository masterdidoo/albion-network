using System.Collections.Generic;

namespace Albion.Network.Messages
{
    public abstract class BaseMessage<TCode> : BaseMessage
    {
        public abstract TCode Code { get; }
    }

    public abstract class BaseMessage
    {
        public abstract void Init(Dictionary<byte, object> parameters);
    }
}