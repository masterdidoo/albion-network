// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

using Albion.Common;
using System;

namespace Albion.Network
{
    public class EventHandler<TEvent> : PacketHandler<EventPacket> where TEvent : BaseEvent
    {
        private readonly EventCodes eventCode;
        private readonly Action<TEvent> action;

        internal EventHandler(EventCodes eventCode, Action<TEvent> action)
        {
            this.eventCode = eventCode;
            this.action = action;
        }

        protected internal override void OnHandle(EventPacket packet)
        {
            if (eventCode != packet.EventCode)
            {
                Next(packet);
            }
            else
            {
                TEvent instance;
                IDataMapper mapper;

                Type eventType = typeof(TEvent);
                DataMapperAttribute attribute = (DataMapperAttribute)Attribute.GetCustomAttribute(eventType, typeof(DataMapperAttribute));
                if (attribute != null)
                {
                    mapper = (IDataMapper)Activator.CreateInstance(attribute.DeserializerType);
                }
                else
                {
                    mapper = new DefaultDataMapper();
                }

                instance = mapper.MapFromParameters<TEvent>(packet.Parameters);

                action.Invoke(instance);
            }
        }
    }
}
