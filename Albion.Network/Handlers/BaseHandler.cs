using System;
using System.Collections.Generic;
using Albion.Network.Messages;

namespace Albion.Network.Handlers
{
    internal abstract class BaseHandler
    {
        public abstract void Run(Dictionary<byte, object> parameters);
    }

    internal class BaseHandler<T> : BaseHandler where T : BaseMessage, new()
    {
        private readonly Action<T> _action;

        public BaseHandler(Action<T> action)
        {
            _action = action;
        }

        public override void Run(Dictionary<byte, object> parameters)
        {
            var obj = new T();
            obj.Init(parameters);
            _action(obj);
        }
    }
}